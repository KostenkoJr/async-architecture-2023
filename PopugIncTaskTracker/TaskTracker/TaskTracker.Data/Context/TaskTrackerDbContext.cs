using Microsoft.EntityFrameworkCore;
using TaskTracker.Data.Models;
using Task = TaskTracker.Data.Models.Task;

namespace TaskTracker.Data.Context;

public class TaskTrackerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=kostenko;Password=kostenko;Host=localhost;Port=5455;Database=task-tracker;SearchPath=tracker;");
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("tracker");
    }
}