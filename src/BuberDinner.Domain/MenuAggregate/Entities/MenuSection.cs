using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.MenuAggregate.ValueObjects;

namespace BuberDinner.Domain.MenuAggregate.Entities;

public sealed class MenuSection : Entity<MenuSectionId>
{
    private readonly List<MenuItem>? _menuItems = new();

    public string Name { get; }

    public string Description { get; }

    public IReadOnlyList<MenuItem>? MenuItems => _menuItems?.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuSection"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    public MenuSection(
        MenuSectionId id,
        string name,
        string description,
        List<MenuItem>? menuItems = null) : base(id)
    {
        Name = name;
        Description = description;
        _menuItems = menuItems;
    }

    public static MenuSection Create(
    string name,
    string description,
    List<MenuItem>? items = null)
    {
        return new(MenuSectionId.CreateUnique(),
                   name,
                   description,
                   items);
    }
}