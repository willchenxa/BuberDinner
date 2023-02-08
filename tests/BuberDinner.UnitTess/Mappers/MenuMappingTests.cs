using Mapster;
using MapsterMapper;
using BuberDinner.Api.Mapping;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Menus;

using FluentAssertions;

namespace BuberDinner.UnitTess.Mappers;

public class MenuMappingTests
{
    private readonly IMapper _mapper;

    public MenuMappingTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MenuMappingConfig).Assembly);
        _mapper = new Mapper(config);
    }

    [Fact]
    public void MenuMapper_Should_Success()
    {
        // Arrange
        var request = new CreateMenuRequest("Menu1", "Create the first menu", new List<MenuSection>
        {
            new MenuSection("Starter", "Soup", new List<MenuItem>
            {
                new MenuItem("Pumpkin Soup", "Delicious Pumpkin Soup", 5.99m)
            })
        });
        
        // Act
        var command = _mapper.Map<CreateMenuCommand>(request);
        
        // Assert
        command.Should().NotBeNull();
        command.Name.Should().BeEquivalentTo("Menu1");
        command.Sections.Count().Should().Be(1);
        command.Sections.First().Name.Should().BeEquivalentTo("Starter");
        command.Sections.First().MenuItems.Count().Should().Be(1);
        command.Sections.First().MenuItems.First().Name.Should().BeEquivalentTo("Pumpkin Soup");
    }
}