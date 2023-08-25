using Accounting.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Data.Context;

public class AccountingDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Entities.Task> Tasks { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=kostenko;Password=kostenko;Host=localhost;Port=5455;Database=task-tracker;SearchPath=accounting;");
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("accounting");
    }
}