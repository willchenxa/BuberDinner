namespace BuberDinner.Contracts.Menus;

public record MenuResponse(string Id,
                           string Name,
                           string Description,
                           float? AverageRating,
                           List<MenuSectionResponse> MenuSection,
                           string HostId,
                           List<string> DinnerIds,
                           List<string> MenuReviewIds,
                           DateTime CreatedDateTime,
                           DateTime UpdatedDateTime);

public record MenuSectionResponse(string Id,
                                  string Name,
                                  string Description,
                                  List<MenuItemResponse> MenuItems);

public record class MenuItemResponse(string Id,
                                     string Name,
                                     string Description,
                                     decimal Price);