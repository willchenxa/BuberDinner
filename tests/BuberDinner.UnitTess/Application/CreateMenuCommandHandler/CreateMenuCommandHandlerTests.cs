using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Menus;

using FluentAssertions;

using NSubstitute;

namespace BuberDinner.UnitTess.Application.MenuHandler;

public class CreateMenuCommandHandlerTests
{
    private readonly IMenuRepository _menuRepository;
    private readonly CreateMenuCommandHandler _handler;

    public CreateMenuCommandHandlerTests(BuberDinnerDbContext context)
    {
        _menuRepository = new MenuRepository(context);
        _handler = new CreateMenuCommandHandler(_menuRepository);
    }

    [Fact]
    public async Task CreateMenu_WhenCommandIsValid_ReturnSuccessful()
    {
        // Arrange
        var hostId = Guid.NewGuid();
        var command = new CreateMenuCommand(hostId.ToString(), "Delicious dinner", "A very delicious dinner menu", new List<MenuSectionCommand>());
        // Act
        var actionResult = await _handler.Handle(command, new CancellationToken());
        
        // Action
        actionResult.Should().NotBeNull();
        actionResult.IsError.Should().BeFalse();
        actionResult.Value.HostId.Value.ToString().Should().BeEquivalentTo(hostId.ToString());
        actionResult.Value.Name.Should().BeEquivalentTo("Delicious dinner");
        actionResult.Value.Description.Should().BeEquivalentTo("A very delicious dinner menu");
    }
}