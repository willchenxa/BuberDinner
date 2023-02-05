using System.Collections.Generic;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Menu.ValueObjects;

namespace BuberDinner.Domain.Menu.Entities;


public sealed class MenuSection : Entity<MenuSectionId>
{
    public readonly List<MenuItem> _menuItems = new();

    public MenuSection(
        MenuSectionId id,
        string name,
        string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }
    public IReadOnlyList<MenuItem> Items => _menuItems.AsReadOnly();

    public static MenuSection Create(string name,
                                     string description)
    {
        return new(MenuSectionId.CreateUnique(),
                   name,
                   description);
    }
}
