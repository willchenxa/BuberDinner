using System.Net;
using System.Net.Http.Headers;
using System.Text;

using BuberDinner.Api;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Contracts.Menus;
using BuberDinner.Fakes.IntegrationTests.MockData;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

namespace BuberDinner.Fakes.IntegrationTests.Controllers;

public class MenusControllerTests : IClassFixture<WebApplicationFactory<WebMarker>>
{
    private readonly HttpClient _httpClient;
    
    public MenusControllerTests(WebApplicationFactory<WebMarker> factory)
    {
        _httpClient = factory.CreateDefaultClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    [Fact]
    public async Task Response_WhenRequestIsNotValid_ReturnBadRequest()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {TestData.Token}");

        var requst = JsonConvert.SerializeObject(
            new CreateMenuRequest("Starter", "a delicious starter", new List<MenuSection>()));
        var content = new StringContent(requst, Encoding.UTF8, "application/json");
        var requestBody = new HttpRequestMessage(HttpMethod.Post, $"host/{Guid.NewGuid()}/menus") { Content = content };
        
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
    public async Task Response_WhenRequestIsValid_ReturnSuccessful()
    {
        // Arrange
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {TestData.Token}");

        var requst = JsonConvert.SerializeObject(
            new CreateMenuRequest("Dinner", "a delicious dinner", new List<MenuSection>
            {
                new MenuSection("Starter", "a delicious starter", new List<MenuItem>
                {
                    new MenuItem("Soup", "A delicious Soup", 5.99m)
                })
            }));
        var content = new StringContent(requst, Encoding.UTF8, "application/json");
        var hostId = Guid.NewGuid();
        var requestBody = new HttpRequestMessage(HttpMethod.Post, $"host/{hostId}/menus") { Content = content };
        
        // Act
        var actionResult = await _httpClient.SendAsync(requestBody);
        
        // Assert
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = JsonConvert.DeserializeObject<MenuResponse>(await actionResult.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
        response.Name.Should().BeEquivalentTo("Dinner");
        response.MenuSection.Count.Should().Be(1);
        response.MenuSection.First().Name.Should().BeEquivalentTo("Starter");
    }
}