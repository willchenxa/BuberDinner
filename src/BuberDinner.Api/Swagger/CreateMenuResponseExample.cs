using Swashbuckle.AspNetCore.Filters;
using BuberDinner.Contracts.Menus;

namespace BuberDinner.Api.Swagger;

public class CreateMenuResponseExample : IExamplesProvider<MenuResponse>
{
    public MenuResponse GetExamples()
    {
        return new MenuResponse(
            Guid.NewGuid().ToString(),
            "David Noodles",
            "A delicious menu",
            4.8f,
            new List<MenuSectionResponse>
            {
                new MenuSectionResponse(
                    Guid.NewGuid().ToString(),
                    "Malatang", 
                    "A very hot mix soup",
                    new List<MenuItemResponse>
                    {
                        new MenuItemResponse(
                            Guid.NewGuid().ToString(),
                            "Hot soup",
                            "3 star hot soup",
                            19.99m)
                    })
            }, 
            Guid.NewGuid().ToString(),
            new List<string>
            {
                Guid.NewGuid().ToString()
            },
            new List<string>{Guid.NewGuid().ToString()}, 
            DateTime.Now, 
            DateTime.Now);
    }
}