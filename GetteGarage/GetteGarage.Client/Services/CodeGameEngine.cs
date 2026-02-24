using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using Blazored.LocalStorage;

public class CodeGameEngine : IDisposable
{
    private readonly ILocalStorageService _localStorage;
    private const string SaveKey = "dev_sim_save_v2";

    // The runtime state
    public List<CodeGameUpgradeDefinition> Upgrades { get; private set; } = new();
    public CodeGameState State { get; private set; } = new();
    public event Action OnStateChanged;

    private System.Timers.Timer _gameLoopTimer;
    private System.Timers.Timer _autoSaveTimer;
    private Random _rng = new();

    public CodeGameEngine(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        InitializeUpgrades(); // Load defaults immediately

        // Game Loop (10 ticks/sec)
        _gameLoopTimer = new System.Timers.Timer(100);
        _gameLoopTimer.Elapsed += GameTick;
        _gameLoopTimer.Start();

        // Auto Save (Every 30 seconds)
        _autoSaveTimer = new System.Timers.Timer(30000);
        _autoSaveTimer.Elapsed += async (s, e) => await SaveGameAsync();
        _autoSaveTimer.Start();
    }

    // --- SAVE / LOAD SYSTEM ---

    public async Task InitializeAsync()
    {
        await LoadGameAsync();
    }

    private async Task LoadGameAsync()
    {
        bool exists = await _localStorage.ContainKeyAsync(SaveKey);
        if (exists)
        {
            try
            {
                var loadedState = await _localStorage.GetItemAsync<CodeGameState>(SaveKey);
                if (loadedState != null)
                {
                    State = loadedState;

                    // Re-hydrate the Upgrade Definitions with the loaded Levels
                    foreach (var def in Upgrades)
                    {
                        var saved = State.Upgrades.FirstOrDefault(u => u.Id == def.Id);
                        if (saved != null)
                        {
                            def.Level = saved.Level;
                            def.IsUnlocked = saved.IsUnlocked;
                        }
                    }
                    AddLog("System State Restored.");
                }
            }
            catch
            {
                AddLog("Save file corrupted. Starting fresh.");
            }
        }
        else
        {
            AddLog("New Project Initialized.");
        }
        OnStateChanged?.Invoke();
    }

    public async Task SaveGameAsync()
    {
        // Sync the runtime definitions back to the simple save state
        State.Upgrades = Upgrades.Select(u => new CodeGameUpgradeState 
        { 
            Id = u.Id, 
            Level = u.Level, 
            IsUnlocked = u.IsUnlocked 
        }).ToList();

        State.LastSaveTime = DateTime.UtcNow;
        await _localStorage.SetItemAsync(SaveKey, State);
        // Optional: AddLog("Auto-saved."); 
        OnStateChanged?.Invoke();
    }

    public async Task HardResetAsync()
    {
        await _localStorage.RemoveItemAsync(SaveKey);
        State = new CodeGameState();
        InitializeUpgrades(); // Reset levels to 0
        AddLog("HARD RESET: System formatted.");
        await SaveGameAsync();
        OnStateChanged?.Invoke();
    }

    // --- GAMEPLAY LOGIC ---

    private void InitializeGame()
    {
        if (!State.Modules.Any())
        {
            // The Root Node
            var main = new CodeGameCodeModule { Name = "Main.exe", Type = "Core", BaseProduction = 1 };
            State.Modules.Add(main);
            AddLog("Project initialized. Main.exe created.");
        }
    }

    private void GameTick(object? sender, ElapsedEventArgs e)
    {
        if (State.Modules.Count == 0) InitializeGame();

        double totalTickProduction = 0;
        double totalSystemHealth = 0;
    
        // Create a copy of the list for iteration in case we modify it (though we won't delete here)
        var modules = State.Modules.ToList(); 

        foreach (var mod in modules)
        {
            // --- LOGIC FIX: HEALTH CLAMPING & REBOOT ---
            if (mod.BugCount > 0)
            {
                // Bugs rot the module rapidly
                mod.Health -= (0.5 * mod.BugCount); 
            }
            else
            {
                if (mod.Health <= 0) 
                {
                    // REBOOT MODE: If crashed but no bugs, heal fast
                    mod.Health += 2.0; 
                }
                else if (mod.Health < 100) 
                {
                    // MAINTENANCE MODE: Heal slowly
                    mod.Health += 0.1; 
                }
            }

            // HARD CLAMP: Never go below 0 or above 100
            if (mod.Health < 0) mod.Health = 0;
            if (mod.Health > 100) mod.Health = 100;

            

            // B. Calculate Dependency Penalty
            double dependencyEfficiency = 1.0;
            foreach (var depId in mod.Dependencies)
            {
                var parent = State.Modules.FirstOrDefault(m => m.Id == depId);
                if (parent != null)
                {
                    // If parent is crashed, I produce NOTHING.
                    // If parent is 50% health, I am 50% efficient.
                    double parentFactor = Math.Max(0, parent.Health / 100.0);
                    dependencyEfficiency *= parentFactor;
                }
            }

            // C. Calculate Output
            double healthFactor = Math.Max(0, mod.Health / 100.0);
            
            // If the module itself is crashed, it produces 0.
            // Otherwise: Base * Level * OwnHealth * ParentHealth
            mod.EffectiveProduction = mod.BaseProduction * mod.Level * healthFactor * dependencyEfficiency;

            totalTickProduction += mod.EffectiveProduction;
            totalSystemHealth += mod.Health;

            }

        // 2. APPLY TO GLOBAL STATE
        // Note: We divide by 10 because tick is 100ms
        State.LinesOfCode += (totalTickProduction / 10.0);
        State.LifetimeLOC += (totalTickProduction / 10.0);

        // 3. GLOBAL EVENTS
        // Calculate global stability for UI
        double avgHealth = State.Modules.Count > 0 ? totalSystemHealth / State.Modules.Count : 100;
        
        // Spawn Bugs in random modules based on Tech Debt
        double bugChance = 0.005 + (State.TechnicalDebt * 0.001);
        if (_rng.NextDouble() < bugChance)
        {
            // Pick a random module to infect
            var target = State.Modules[_rng.Next(State.Modules.Count)];
            target.BugCount++;
            AddLog($"Bug detected in {target.Name}!");
        }

        OnStateChanged?.Invoke();
    }

    public void WriteCode()
    {
        double clickPower = 1 + Upgrades.Sum(u => u.Level * u.ClickPower);
        State.LinesOfCode += clickPower;
        State.LifetimeLOC += clickPower;

        // Manual coding creates slight bugs (5% chance)
        if (_rng.NextDouble() < 0.05) SpawnBug();
        OnStateChanged?.Invoke();
    }

    public void DebugCode()
    {
        if (State.ActiveBugs > 0)
        {
            State.ActiveBugs--;
            if (State.TechnicalDebt > 0) State.TechnicalDebt = Math.Max(0, State.TechnicalDebt - 0.5);
            AddLog("Bug squashed. Debt reduced.");
            OnStateChanged?.Invoke();
        }
    }

    public void BuyUpgrade(string id)
    {
        var item = Upgrades.FirstOrDefault(u => u.Id == id);
        if (item != null && State.LinesOfCode >= item.CurrentCost)
        {
            State.LinesOfCode -= item.CurrentCost;
            item.Level++;
            
            // Add Debt immediately
            State.TechnicalDebt += item.DebtGeneration;
            
            AddLog($"Acquired {item.Name}.");
            OnStateChanged?.Invoke();
        }
    }

    private void SpawnBug()
    {
        State.ActiveBugs++;
        AddLog("RUNTIME ERROR!");
    }

    private void AddLog(string msg)
    {
        State.Logs.Insert(0, msg);
        if (State.Logs.Count > 6) State.Logs.RemoveAt(State.Logs.Count - 1);
    }

    private void InitializeUpgrades()
    {
        // These are the "Rules" of the game.
        Upgrades = new List<CodeGameUpgradeDefinition>
        {
            new CodeGameUpgradeDefinition { Id="coffee", Name="Caffeine", BaseCost=15, LOCPerSecond=0, ClickPower=1, DebtGeneration=0.1, Description="Type faster. +0.1 Debt." },
            new CodeGameUpgradeDefinition { Id="intern", Name="Hire Intern", BaseCost=100, LOCPerSecond=5, DebtGeneration=2.0, Description="Cheap labor. +2.0 Debt." },
            new CodeGameUpgradeDefinition { Id="copilot", Name="AI Copilot", BaseCost=500, LOCPerSecond=25, DebtGeneration=5.0, Description="Fast but hallucinating. +5.0 Debt." },
            new CodeGameUpgradeDefinition { Id="senior", Name="Senior Dev", BaseCost=2000, LOCPerSecond=10, DebtGeneration=-0.5, Description="Cleans mess. -0.5 Debt." }
        };
    }

    public void Dispose()
    {
        _gameLoopTimer?.Stop();
        _autoSaveTimer?.Stop();
    }
    public void AddModule(string type)
    {
        double cost = 100 * Math.Pow(1.5, State.Modules.Count);
        if (State.LinesOfCode >= cost)
        {
            State.LinesOfCode -= cost;
            
            var newMod = new CodeGameCodeModule 
            { 
                Name = $"{type}_{_rng.Next(100, 999)}", 
                Type = type, 
                BaseProduction = 5 * State.Modules.Count // Newer modules are stronger
            };

            // Auto-link to a random existing module (creates the dependency web)
            if (State.Modules.Count > 0)
            {
                var parent = State.Modules[_rng.Next(State.Modules.Count)];
                newMod.Dependencies.Add(parent.Id);
            }

            State.Modules.Add(newMod);
            AddLog($"Created {newMod.Name}. Linked to {State.Modules.Count-1} modules.");
        }
    }

    public void FixModule(Guid id)
    {
        var mod = State.Modules.FirstOrDefault(m => m.Id == id);
        if (mod != null && mod.BugCount > 0)
        {
            mod.BugCount--;
            mod.Health += 10; // Immediate patch boost
            if (mod.Health > 100) mod.Health = 100;
            
            // Fixing bugs reduces global debt slightly
            State.TechnicalDebt = Math.Max(0, State.TechnicalDebt - 0.1);
        }
    }
    public void DeleteModule(Guid id)
    {
        var mod = State.Modules.FirstOrDefault(m => m.Id == id);
        if (mod != null)
        {
            // 1. Remove the module
            State.Modules.Remove(mod);

            // 2. Clean the graph: Remove this ID from anyone who depends on it
            foreach (var other in State.Modules)
            {
                if (other.Dependencies.Contains(id))
                {
                    other.Dependencies.Remove(id);
                }
            }
            
            // 3. Refund? (Optional - for now just delete)
            AddLog($"Terminated module: {mod.Name}");
            OnStateChanged?.Invoke();
        }
    }
}