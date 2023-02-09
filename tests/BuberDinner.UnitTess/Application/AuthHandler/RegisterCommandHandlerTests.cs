using FluentAssertions;
using NSubstitute;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.User;

namespace BuberDinner.UnitTess.Application.AuthHandler;

public class RegisterCommandHandlerTests
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly RegisterCommandHandler _registerCommandHandler;

    public RegisterCommandHandlerTests()
    {
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _userRepository = Substitute.For<IUserRepository>();

        _registerCommandHandler = new RegisterCommandHandler(_jwtTokenGenerator, _userRepository);
    }
    
    [Fact]
    public async Task RegisterSuccessful_WhenRequest_IsValid()
    {
        // Arrange
        var command = new RegisterCommand("William", "Chen", "william.chen@email.address", "Passw0rd");
        _jwtTokenGenerator.GenerateToken(Arg.Any<User>()).Returns("Token");
        
        // Act
        var result = await _registerCommandHandler.Handle(command, new CancellationToken());
        
        // Assert
        result.Should().NotBeNull();
        result.Value.User.FirstName.Should().BeEquivalentTo("William");
        result.Value.User.Email.Should().BeEquivalentTo("william.chen@email.address");
        result.Value.Token.Should().BeEquivalentTo("Token");
    }
    
    [Fact]
    public async Task Register_WhenUserExists_ReturnError()
    {
        // Arrange
        var command = new RegisterCommand("William", "Chen", "william.chen@email.address", "Passw0rd");
        _userRepository.GetUserByEmail(Arg.Any<string>()).Returns(new User());
        
        // Act
        var result = await _registerCommandHandler.Handle(command, new CancellationToken());
        
        // Assert
        result.Should().NotBeNull();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().Code.Should().BeEquivalentTo("User.DuplicateEmail");
        result.Errors.First().Description.Should().BeEquivalentTo("Email is already in use.");
    }
}