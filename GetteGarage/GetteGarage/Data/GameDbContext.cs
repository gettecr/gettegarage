using Microsoft.EntityFrameworkCore;
using GetteGarage.Models;

namespace GetteGarage.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

    // This creates a table named "FishingRecords"
    public DbSet<FishingRecord> FishingRecords { get; set; }
    // creates a table named BlogCache
    public DbSet<BlogCacheRecord> BlogCache { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<FishingRecord>()
            .HasIndex(r => new { r.FishName, r.Length });
    }
}