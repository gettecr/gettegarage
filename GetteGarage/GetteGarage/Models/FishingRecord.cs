namespace GetteGarage.Models;

public class FishingRecord
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = "Anonymous";
    public string FishName { get; set; } = "";
    public double Length { get; set; } // e.g., 45.2 cm
    public string RodUsed { get; set; } = "";
    public DateTime DateCaught { get; set; } = DateTime.UtcNow;
}