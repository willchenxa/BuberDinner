using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.MenuAggregate;

namespace BuberDinner.Infrastructure.Persistence.Menus;

public class MenuRepository : IMenuRepository
{
    private readonly BuberDinnerDbContext _buberDinnerDbContext;

    public MenuRepository(BuberDinnerDbContext buberDinnerDbContext)
    {
        _buberDinnerDbContext = buberDinnerDbContext;
    }
    
    public void Add(Menu menu)
    {
        _buberDinnerDbContext.Add(menu);
        _buberDinnerDbContext.SaveChanges();
    }
}