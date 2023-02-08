using FluentAssertions;
using NSubstitute;
using MediatR;
using MapsterMapper;
using BuberDinner.Api.Controllers;
using BuberDinner.Contracts.Menus;

namespace BuberDinner.UnitTess.Controllers;

public class MenuControllersTests
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly MenusController _controller;
    
    public MenuControllersTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<ISender>();
        _controller = new MenusController(_mapper, _mediator);
    }

    [Fact]
    public async Task CreateMenu_Returns_Success_When_ValidInput()
    {
        // Arrange
        var request = new CreateMenuRequest("TestMenu", "It's a test menu",
            new List<MenuSection>
            {
                new MenuSection("Test Section", "Test Description",
                    new List<MenuItem> { new MenuItem("test item", "tes description", 5.99m) })
            });
        
        // Act
        var response = await _controller.CreateMenu(request, Guid.NewGuid().ToString());
        
        // Assert
        response.Should().NotBeNull();
    }
}