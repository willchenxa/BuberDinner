using BuberDinner.Domain.MenuAggregate;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Domain.User;

using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence.Common;

public class BubberDinnerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Menu> Menu { get; set; }
    public DbSet<MenuSection> MenuSections { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    public BubberDinnerDbContext(DbContextOptions<BubberDinnerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        modelBuilder.Entity<Menu>().HasKey(menu => menu.Id);
        modelBuilder.Entity<MenuSection>().HasKey(menu => menu.Id);
        modelBuilder.Entity<MenuItem>().HasKey(menu => menu.Id);

        modelBuilder.Entity<Menu>()
            .HasMany(menu => menu.MenuSection);
        modelBuilder.Entity<MenuSection>()
            .HasMany(menuSection => menuSection.MenuItems);
        
        base.OnModelCreating(modelBuilder);
    }
}