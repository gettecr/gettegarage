public class CodeGameState
{
    public double LinesOfCode { get; set; } = 0;
    public double LifetimeLOC { get; set; } = 0;
    public int ActiveBugs { get; set; } = 0;
    public double TechnicalDebt { get; set; } = 0;
    public DateTime LastSaveTime { get; set; } = DateTime.UtcNow;

    public List<CodeGameCodeModule> Modules { get; set; } = new();

    // We only save the "State" of upgrades (Level, IsUnlocked). 
    // The logic (Cost, Name) comes from the code defaults to prevent hacking/version mismatches.
    public List<CodeGameUpgradeState> Upgrades { get; set; } = new();
    public List<string> Logs { get; set; } = new();
}

// Separate the "Save Data" from the "Game Logic"
public class CodeGameUpgradeState
{
    public string Id { get; set; } = "";
    public int Level { get; set; }
    public bool IsUnlocked { get; set; }
}

// This class is used by the Engine at runtime
public class CodeGameUpgradeDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public double BaseCost { get; set; }
    public double CostMultiplier { get; set; } = 1.3;
    
    // Runtime Stats
    public int Level { get; set; }
    public double LOCPerSecond { get; set; }
    public double ClickPower { get; set; }
    public double DebtGeneration { get; set; }

    public double CurrentCost => BaseCost * Math.Pow(CostMultiplier, Level);
    public bool IsUnlocked { get; set; }
}

public class CodeGameCodeModule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Type { get; set; }  = "";// "Frontend", "Backend", "Database", "Utility"
    
    // Stats
    public double BaseProduction { get; set; }
    public double Health { get; set; } = 100.0; // 0% = Crashed, 100% = Stable
    public int BugCount { get; set; } = 0;
    public int Level { get; set; } = 1;

    // The Graph
    public List<Guid> Dependencies { get; set; } = new();
    
    // Calculated realtime (not saved)
    public double EffectiveProduction { get; set; } 
    public bool IsCrashed => Health <= 0;
}