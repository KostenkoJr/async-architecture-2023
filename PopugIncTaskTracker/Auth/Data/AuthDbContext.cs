using Auth.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Data;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=kostenko;Password=kostenko;Host=localhost;Port=5455;Database=task-tracker;");
    }
}