using System.Text.Json;
using GetteGarage.Models;

namespace GetteGarage.Services;

public class HighScoreService
{
    private readonly string _filePath = "highscores.json";
    
    public List<GameScore> GetTopScores(string gameName)
    {
        var all = LoadScores();
        return all.Where(s => s.GameName == gameName)
                  .OrderByDescending(s => s.Score)
                  .Take(10)
                  .ToList();
    }

    public void AddScore(GameScore score)
    {
        var all = LoadScores();
        all.Add(score);
        SaveScores(all);
    }

    private List<GameScore> LoadScores()
    {
        if (!File.Exists(_filePath)) return new List<GameScore>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<GameScore>>(json) ?? new List<GameScore>();
    }

    private void SaveScores(List<GameScore> scores)
    {
        var json = JsonSerializer.Serialize(scores);
        File.WriteAllText(_filePath, json);
    }
}