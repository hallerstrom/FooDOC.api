using System;
using FooDOC.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FooDOC.api.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<TempCCP> TempCCPs { get; set; }
    public DbSet<Product> Products { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Skapa en hasher-instans
        var hasher = new PasswordHasher<IdentityUser>(); 

        // Skapa admin-användaren
        var adminUser = new IdentityUser
        {
            Id = "1", 
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

        modelBuilder.Entity<IdentityUser>().HasData(adminUser);

        modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>
        {
        Id = 1,
        UserId = "1",
        ClaimType = "role",
        ClaimValue = "admin"
        });

        // Skapa user-användaren
        var userUser = new IdentityUser 
        {
            Id = "2", 
            UserName = "user",
            NormalizedUserName = "USER",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        userUser.PasswordHash = hasher.HashPassword(userUser, "User123!");

        modelBuilder.Entity<IdentityUser>().HasData(userUser); 

        // Seeda kontrollpunkter som mockdata
        modelBuilder.Entity<TempCCP>().HasData(
            new TempCCP { Id = 1, Product = "Hel Kyckling", Temp = 99.54, CreatedAt = DateTime.UtcNow },
            new TempCCP { Id = 2, Product = "Kamben", Temp = 30.55, CreatedAt = DateTime.UtcNow }
        );


        // Seeda produkter som mock
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name ="Hel Kyckling"},
            new Product { Id = 2, Name ="Kycklingben"},
            new Product { Id = 3, Name ="Kamben"}
        );
    }
}

