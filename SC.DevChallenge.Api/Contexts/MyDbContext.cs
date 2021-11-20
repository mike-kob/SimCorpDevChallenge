using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using SC.DevChallenge.Api.Models;

namespace SC.DevChallenge.Api.Contexts
{
  public interface IDbContext : IDisposable
  {
    DbSet<PriceValues> PriceSet { get; }
  }

  public class MyDbContext : DbContext, IDbContext
  {
    public DbSet<PriceValues> PriceSet { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Filename=TestDatabase.db", options =>
      {
        options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
      });
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<PriceValues>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
      });
      base.OnModelCreating(modelBuilder);
    }
  }
}