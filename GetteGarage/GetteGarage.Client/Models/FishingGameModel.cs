namespace GetteGarage.Client.Models;

public enum GameState { Idle, Casting, Waiting, Minigame }
public enum RodType { Basic, Qte, Ascii, Rhythm }
public enum QteDifficulty { Easy, Medium,Hard, Expert } 

public class VisualTheme
{
    public string BackgroundUrl { get; set; } = "";
    public string WaterOverlayUrl { get; set; } = ""; 
    public string FishermanIdleUrl { get; set; } = ""; 
    public string FishermanCastUrl { get; set; } = ""; 
}

public class RodDef 
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public RodType Type { get; set; }
    public QteDifficulty Difficulty { get; set; } = QteDifficulty.Easy; 
    public int UnlockCost { get; set; }
    public bool IsUnlocked { get; set; }
    
    public string BobberSpriteUrl { get; set; } = ""; 
    public string IconUrl { get; set; } = ""; 
}

public class FishDef 
{
    public string Name { get; set; } = "";
    public string Emoji { get; set; } = "🐟";
    public double MinLength { get; set; }
    public double MaxLength { get; set; }
    public string SpriteUrl { get; set; } = "";
    public double Rarity {get; set;} = 1.0;
    public double BaseValue { get; set; }
}

public class CaughtFish 
{
    public string Name { get; set; } = "";
    public string Emoji { get; set; } = "🐟";
    public double Length { get; set; }
    public string SpriteUrl { get; set; } = "";
    public double Value { get; set; }
    public bool IsNewRecord { get; set; }
}

public class PlayerFishRecord
{
    public string Name { get; set; }
    public int Count { get; set; }
    public double MaxLength { get; set; }
}

public class FishingSaveData
{
    public int TotalFish { get; set; }
    public double TotalMoney { get; set; }
    public Dictionary<string, PlayerFishRecord> PersonalBests { get; set; } = new();
    
    public List<string> UnlockedRodIds { get; set; } = new();
    public string ActiveRodId { get; set; } = "";
}