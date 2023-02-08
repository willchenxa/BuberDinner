using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

using BuberDinner.Api;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Fakes.IntegrationTests.MockData;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

using Newtonsoft.Json;

namespace BuberDinner.Fakes.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<WebMarker>>
{
    private readonly HttpClient _httpClient;
    
    public AuthControllerTests(WebApplicationFactory<WebMarker> factory)
    {
        _httpClient = factory.CreateDefaultClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {TestData.Token}");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    [Fact]
    public async Task Response_WhenRequestIsNotValid_ReturnBadRequest()
    {
        // Arrange
        var requst = JsonConvert.SerializeObject(
                new RegisterRequest("Will", "", "will.smith@email.address", "Passw0rd"));
        var content = new StringContent(requst, Encoding.UTF8, "application/json");
        var requestBody = new HttpRequestMessage(HttpMethod.Post, "auth/register") { Content = content };
        
        // Act
        var actionResult = await _httpClient.SendAsync(requestBody);
        
        // Assert
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = JsonConvert.DeserializeObject<ProblemDetails>(await actionResult.Content.ReadAsStringAsync());
        error.Should().NotBeNull();
        error.Title.Should().BeEquivalentTo("One or more validation errors occurred.");
        error.Type.Should().BeEquivalentTo("https://tools.ietf.org/html/rfc7231#section-6.5.1");
    }
    
    [Fact]
    public async Task Response_WhenUserAlreadyExist_ReturnConflict()
    {
        // Arrange
        var registerRequest = JsonConvert.SerializeObject(
            new RegisterRequest("Tom", "Cruise", "tom.cruise@email.address", "Passw0rd"));
        var registeredContent = new StringContent(registerRequest, Encoding.UTF8, "application/json");
        var registerRequestBody = new HttpRequestMessage(HttpMethod.Post, "auth/register") { Content = registeredContent };
        
        var request = JsonConvert.SerializeObject(
            new RegisterRequest("Tom", "Cruise", "tom.cruise@email.address", "Passw0rd1"));
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        var requestBody = new HttpRequestMessage(HttpMethod.Post, "auth/register") { Content = content };

        // Act
        var registeredResult = await _httpClient.SendAsync(registerRequestBody);
        var actionResult = await _httpClient.SendAsync(requestBody);

        // Assert
        registeredResult.Should().NotBeNull();
        registeredResult!.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = JsonConvert.DeserializeObject<AuthenticationResponse>(await registeredResult.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
        response.FirstName.Should().BeEquivalentTo("Tom");
        response.LastName.Should().BeEquivalentTo("Cruise");
        
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var error = JsonConvert.DeserializeObject<ProblemDetails>(await actionResult.Content.ReadAsStringAsync());
        error.Should().NotBeNull();
        error.Title.Should().BeEquivalentTo("Email is already in use.");
        error.Type.Should().BeEquivalentTo("https://tools.ietf.org/html/rfc7231#section-6.5.8");
    }
    
    [Fact]
    public async Task Response_WhenUserLoginError_ReturnBadRequest()
    {
        // Arrange
        var registerRequest = JsonConvert.SerializeObject(
            new RegisterRequest("William", "Chen", "william.chen@email.address", "Passw0rd"));
        var registeredContent = new StringContent(registerRequest, Encoding.UTF8, "application/json");
        var registerRequestBody = new HttpRequestMessage(HttpMethod.Post, "auth/register") { Content = registeredContent };
        
        var request = JsonConvert.SerializeObject(
            new LoginRequest("william.chen@email.address", "Passw0rd1"));
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        var requestBody = new HttpRequestMessage(HttpMethod.Post, "auth/login") { Content = content };

        // Act
        var registeredResult = await _httpClient.SendAsync(registerRequestBody);
        var actionResult = await _httpClient.SendAsync(requestBody);

        // Assert
        registeredResult.Should().NotBeNull();
        registeredResult!.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = JsonConvert.DeserializeObject<AuthenticationResponse>(await registeredResult.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
        response.FirstName.Should().BeEquivalentTo("William");
        response.LastName.Should().BeEquivalentTo("Chen");
        
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var error = JsonConvert.DeserializeObject<ProblemDetails>(await actionResult.Content.ReadAsStringAsync());
        error.Should().NotBeNull();
        error.Title.Should().BeEquivalentTo("Invalid credentials.");
        error.Type.Should().BeEquivalentTo("https://tools.ietf.org/html/rfc7235#section-3.1");
    }
    
    [Fact]
    public async Task Response_WhenRegisterRequestIsValid_ReturnResult()
    {
        // Arrange
        var requst = JsonConvert.SerializeObject(
            new RegisterRequest("Tom", "Hanks", "tom.hanks@email.address", "Passw0rd"));
        var content = new StringContent(requst, Encoding.UTF8, "application/json");
        var requestBody = new HttpRequestMessage(HttpMethod.Post, "auth/register") { Content = content };
        
        // Act
        var actionResult = await _httpClient.SendAsync(requestBody);
        
        // Assert
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = JsonConvert.DeserializeObject<AuthenticationResponse>(await actionResult.Content.ReadAsStringAsync());
        result.Should().NotBeNull();
        result.FirstName.Should().BeEquivalentTo("Tom");
        result.LastName.Should().BeEquivalentTo("Hanks");
        result.Email.Should().BeEquivalentTo("tom.hanks@email.address");
        Guid.TryParse(result.Id.ToString(), out var guidOutput).Should().BeTrue();
    }
    
    [Fact]
    public async Task Response_WhenLoginRequestIsValid_ReturnResult()
    {
        // Arrange
        var requst = JsonConvert.SerializeObject(
            new LoginRequest("Sheldon.Copper@email.address", "Passw0rd"));
        var content = new StringContent(requst, Encoding.UTF8, "application/json");
        var requestBody = new HttpRequestMessage(HttpMethod.Post, "auth/login") { Content = content };
        
        // Act
        var actionResult = await _httpClient.SendAsync(requestBody);
        
        // Assert
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var result = JsonConvert.DeserializeObject<ProblemDetails>(await actionResult.Content.ReadAsStringAsync());
        result.Should().NotBeNull();
        result.Title.Should().BeEquivalentTo("Invalid credentials.");
    }
}
