using System.Text.Json;
using GetteGarage.Models;

namespace GetteGarage.Services;

public class FishingLeaderboardService
{
    private readonly string _filePath = "fishing_records.json";
    
    // Get the biggest fish of a specific species
    public List<FishingRecord> GetTopCatches(string fishName)
    {
        var all = LoadRecords();
        return all.Where(r => r.FishName == fishName)
                  .OrderByDescending(r => r.Length)
                  .Take(5)
                  .ToList();
    }

    // Check if it's the absolute biggest ever caught globally
    public bool IsWorldRecord(string fishName, double size)
    {
        var all = LoadRecords();
        var currentRecord = all.Where(r => r.FishName == fishName)
                               .OrderByDescending(r => r.Length)
                               .FirstOrDefault();
        
        return currentRecord == null || size > currentRecord.Length;
    }

    public void AddRecord(FishingRecord record)
    {
        var all = LoadRecords();
        // Generate pseudo-ID
        record.Id = all.Any() ? all.Max(r => r.Id) + 1 : 1;
        all.Add(record);
        SaveRecords(all);
    }

    private List<FishingRecord> LoadRecords()
    {
        if (!File.Exists(_filePath)) return new List<FishingRecord>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<FishingRecord>>(json) ?? new List<FishingRecord>();
    }

    private void SaveRecords(List<FishingRecord> records)
    {
        var json = JsonSerializer.Serialize(records);
        File.WriteAllText(_filePath, json);
    }
}