using BuberDinner.Domain.Host.ValueObjects;
using BuberDinner.Domain.MenuAggregate;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Domain.MenuAggregate.ValueObjects;
using BuberDinner.Domain.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuberDinner.Infrastructure.Persistence;

public class BuberDinnerDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Menu> Menu { get; set; } = null!;
    public DbSet<MenuSection> MenuSections { get; set; } = null!;
    public DbSet<MenuItem> MenuItems { get; set; } = null!;

    public BuberDinnerDbContext(DbContextOptions<BuberDinnerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        modelBuilder.Entity<Menu>(menu =>
        {
            menu.HasKey(m => m.Id);
            menu.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                    value => MenuId.CreateUnique());
            
            menu.Property(m => m.Name)
                .HasMaxLength(200);
            menu.Property(m => m.Description)
                .HasMaxLength(200);
            
            menu.Property(m => m.HostId)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                    value => HostId.CreateUnique());
        });
        
        modelBuilder.Entity<MenuSection>(menuSection =>
        {
            menuSection.HasKey(s => s.Id);
            
            menuSection.Property(s => s.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                    value => MenuSectionId.CreateUnique());
            
            menuSection.Property(s => s.Name)
                .HasMaxLength(100);
            menuSection.Property(s => s.Description)
                .HasMaxLength(100);
        });
        modelBuilder.Entity<MenuItem>(menuItem =>
        {
            menuItem.HasKey(item => item.Id);
            menuItem.Property(s => s.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                    value => MenuItemId.CreateUnique());
            
            menuItem.Property(e => e.Description)
                .HasMaxLength(100);
            menuItem.Property(e => e.Name)
                .HasMaxLength(100);
            menuItem.Property(e => e.Price);
        });

        modelBuilder.Entity<Menu>().HasMany(menu => menu.MenuSection);
        modelBuilder.Entity<MenuSection>().HasMany(menuSection => menuSection.MenuItems);
        
        base.OnModelCreating(modelBuilder);
    }
    
    private void ConfigureMenuTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(menu =>
        {
            menu.HasKey(m => m.Id);
            menu.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                    value => MenuId.CreateUnique());

            menu.Property(m => m.Name)
                .HasMaxLength(200);
            menu.Property(m => m.Description)
                .HasMaxLength(200);
            
            menu.Property(m => m.HostId)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value,
                    value => HostId.CreateUnique());

            menu.OwnsMany(menu => menu.MenuSection, section =>
            {
                section.WithOwner().HasForeignKey("menuId");
                section.HasKey("id", "menuId");
            
                section.Property(s => s.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .HasConversion(id => id.Value,
                        value => MenuSectionId.CreateUnique());
            
                section.Property(s => s.Name)
                    .HasMaxLength(100);
                section.Property(s => s.Description)
                    .HasMaxLength(100);

                section.OwnsMany(ms => ms.MenuItems, item =>
                {

                    item.WithOwner().HasForeignKey("menuId", "menuSectionId");
                    item.HasKey("id", "menuId", "menuSectionId");
                    item.Property(s => s.Id)
                        .HasColumnName("id")
                        .ValueGeneratedNever()
                        .HasConversion(id => id.Value,
                            value => MenuItemId.CreateUnique());
                    item.Property(e => e.Description)
                        .HasMaxLength(100);
                    item.Property(e => e.Name)
                        .HasMaxLength(100);
                    item.Property(e => e.Price);
                });

                section.Navigation(s => s.MenuItems).Metadata.SetField("_menuItems");
                section.Navigation(s => s.MenuItems).UsePropertyAccessMode(PropertyAccessMode.Field);
            });
        });
        
        modelBuilder.Entity<Menu>().Metadata.FindNavigation(nameof(Domain.MenuAggregate.Menu.MenuSection))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}