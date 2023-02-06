using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.MenuAggregate.ValueObjects;

namespace BuberDinner.Domain.MenuAggregate.Entities;

public sealed class MenuItem : Entity<MenuItemId>
{
    private MenuItem(
        MenuItemId menuItemId,
        string name,
        string description,
        decimal price) : base(menuItemId)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    public string Name { get; }

    public string Description { get; }

    public decimal Price { get; }

    public static MenuItem Create(
        string name,
        string description,
        decimal price)
    {
        return new(
            MenuItemId.CreateUnique(),
            name,
            description,
            price);
    }
}