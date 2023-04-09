using MessageBroker.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageBroker.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
    
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    
  }

  public DbSet<Topic>? Topics { get; set; }
  public DbSet<Subscription>? Subscriptions { get; set; }
  public DbSet<Message>? Messages { get; set; }
}