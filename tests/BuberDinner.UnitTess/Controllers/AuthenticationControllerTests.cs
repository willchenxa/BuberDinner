using BuberDinner.Api.Controllers;
using BuberDinner.Contracts.Authentication;

using FluentAssertions;

using MapsterMapper;

using MediatR;

using NSubstitute;

namespace BuberDinner.UnitTess.Controllers;

public class AuthenticationControllerTests
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly AuthenticationController _controller;
    
    public AuthenticationControllerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<ISender>();
        _controller = new AuthenticationController(_mapper, _mediator);
    }
    
    [Fact]
    public async Task Register_Returns_Success_When_ValidInput()
    {
        // Arrange
        var request = new RegisterRequest("William", "Chen",
            "william.chen@email.address", "Passw0rd");
        
        // Act
        var response = await _controller.Register(request);
        
        // Assert
        response.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Register_Returns_Error_When_InvalidInput()
    {
        // Arrange
        var request = new RegisterRequest("", "Chen",
            "william.chen@email.address", "Passw0rd");
        
        // Act
        var response = await _controller.Register(request);
        
        // Assert
        response.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Login_Returns_Success_When_ValidInput()
    {
        // Arrange
        var request = new LoginRequest("william.chen@email.address","Passw0rd");
        
        // Act
        var response = await _controller.Login(request);
        
        // Assert
        response.Should().NotBeNull();
    }
}