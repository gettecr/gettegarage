namespace GetteGarage.Models;

public class GameScore
{
    public int Id { get; set; }
    public string GameName { get; set; } = ""; 
    public string PlayerName { get; set; } = "";
    public int Score { get; set; }
    public DateTime DateAchieved { get; set; } = DateTime.UtcNow;
}