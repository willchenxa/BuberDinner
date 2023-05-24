using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Host.ValueObjects;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Domain.MenuAggregate.ValueObjects;
using BuberDinner.Domain.MenuReview.ValueObjects;

namespace BuberDinner.Domain.MenuAggregate;

public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuSection> _menuSection = new();
    //private readonly List<DinnerId> _dinnerIds = new();
    //private readonly List<MenuReviewId> _menuReviewIds = new();
    public string Name { get; }
    public string Description { get; set; }
    public float? AverageRating { get; set; }
    public IReadOnlyList<MenuSection> MenuSection => _menuSection.AsReadOnly();
    public HostId HostId { get; }
    //public IReadOnlyList<DinnerId> DinnerIds => _dinnerIds.AsReadOnly();
    //public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds.AsReadOnly();
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }
    private Menu(MenuId id,
                 HostId hostId,
                 string name,
                 string description,
                 List<MenuSection> menuSection,
                 DateTime createdDateTime,
                 DateTime updatedDateTime,
                 float? averageRating = null) : base(id)
    {
        Name = name;
        HostId = hostId;
        Description = description;
        AverageRating = averageRating;
        _menuSection = menuSection;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Menu Create(
        HostId hostId,
        string name,
        string description,
        List<MenuSection>? section,
        DateTime createdDateTime,
        DateTime updatedDateTime
        )
    {
        return new Menu(
            MenuId.CreateUnique(),
            hostId,
            name,
            description,
            section ?? new(),
            createdDateTime,
            updatedDateTime
            );
    }
}