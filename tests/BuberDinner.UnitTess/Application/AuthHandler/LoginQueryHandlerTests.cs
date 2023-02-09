using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.User;

namespace BuberDinner.UnitTess.Application.AuthHandler;

public class LoginQueryHandlerTests
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly LoginQueryHandler _loginQueryHandler;

    public LoginQueryHandlerTests()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _userRepository = Substitute.For<IUserRepository>();

        _loginQueryHandler = new LoginQueryHandler(_jwtTokenGenerator, _userRepository);
    }
    
    [Fact]
    public async Task LoginSuccessful_WhenRequest_IsValid()
    {
        // Arrange
        var query = new LoginQuery("william.chen@email.address", "Passw0rd");
        _userRepository.GetUserByEmail(Arg.Any<string>()).Returns(new User
        {
            FirstName ="William",
            LastName = "Chen",
            Password = "Passw0rd",
            Email = "william.chen@email.address"
        });
        _jwtTokenGenerator.GenerateToken(Arg.Any<User>()).Returns("Token");

        // Act
        var result = await _loginQueryHandler.Handle(query, new CancellationToken());
        
        // Assert
        result.Should().NotBeNull();
        result.Value.User.FirstName.Should().BeEquivalentTo("William");
        result.Value.User.Email.Should().BeEquivalentTo("william.chen@email.address");
        result.Value.Token.Should().BeEquivalentTo("Token");
    }
    
    [Fact]
    public async Task Login_WhenUserNotExists_ReturnError()
    {
        // Arrange
        var query = new LoginQuery("william.chen@email.address", "Passw0rd");
        _userRepository.GetUserByEmail(Arg.Any<string>()).ReturnsNull();
        
        // Act
        var result = await _loginQueryHandler.Handle(query, new CancellationToken());
        
        // Assert
        result.Should().NotBeNull();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().Code.Should().BeEquivalentTo("Auth.InvalidCredential");
        result.Errors.First().Description.Should().BeEquivalentTo("Invalid credentials.");
    }

    [Fact]
    public async Task Login_WhenUserCredentialIsNotDalid_ReturnError()
    {
        // Arrange
        var query = new LoginQuery("william.chen@email.address", "Passw0rd");
        _userRepository.GetUserByEmail(Arg.Any<string>()).Returns(new User
        {
            FirstName ="William",
            LastName = "Chen",
            Password = "Password",
            Email = "william.chen@emil.address"
        });
        
        // Act
        var result = await _loginQueryHandler.Handle(query, new CancellationToken());
        
        // Assert
        result.Should().NotBeNull();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().Code.Should().BeEquivalentTo("Auth.InvalidCredential");
        result.Errors.First().Description.Should().BeEquivalentTo("Invalid credentials.");
    }
}