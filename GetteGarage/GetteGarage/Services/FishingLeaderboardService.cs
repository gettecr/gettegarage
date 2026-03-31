using GetteGarage.Data;
using GetteGarage.Models;

namespace GetteGarage.Services;

public class FishingLeaderboardService
{
    private readonly GameDbContext _db;

    // Inject the database context
    public FishingLeaderboardService(GameDbContext db)
    {
        _db = db;
    }

    public List<FishingRecord> GetTopCatches(string fishName)
    {
        return _db.FishingRecords
            .Where(r => r.FishName == fishName)
            .OrderByDescending(r => r.Length)
            .Take(5)
            .ToList();
    }

    public bool IsWorldRecord(string fishName, double size)
    {
        var currentRecord = _db.FishingRecords
            .Where(r => r.FishName == fishName)
            .OrderByDescending(r => r.Length)
            .FirstOrDefault();
        
        // If no record exists, or the new size is strictly greater, it's a world record!
        return currentRecord == null || size > currentRecord.Length;
    }

    public void AddRecord(FishingRecord record)
    {
        // EF Core automatically handles generating the new ID
        _db.FishingRecords.Add(record);
        _db.SaveChanges();
    }
}