using GetteGarage.Client.Models;
using System.Linq; // Add this using statement for OrderBy
using Blazored.LocalStorage;


namespace GetteGarage.Client.Services;

public class FishingGameEngine
{
    private readonly ILocalStorageService _localStorage;
    private const string SaveKey = "fish_save_v1";
    public int TotalFishCaught { get; private set; } = 0;
    public List<RodDef> AvailableRods { get; private set; } = new();
    public RodDef ActiveRod { get; private set; }
    public VisualTheme CurrentTheme { get; private set; } = new();

    // NEW: Dictionary to track personal bests
    public Dictionary<string, PlayerFishRecord> PersonalBests { get; private set; } = new();
    
    private Dictionary<RodType, List<FishDef>> _fishEcosystem = new();
    public event Action OnStateChanged;

    public FishingGameEngine(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public void Initialize()
    {
        CurrentTheme = new VisualTheme();

        AvailableRods = new List<RodDef>
        {
            new RodDef { Id="rod_1", Name="Basic Rod", Description="1 Click Catch", Type=RodType.Basic, UnlockCost=0, IsUnlocked=true },
            new RodDef { Id="rod_qte_easy", Name="Novice QTE", Description="Home Row Keys", Type=RodType.Qte, Difficulty=QteDifficulty.Easy, UnlockCost=1 },
            new RodDef { Id="rod_qte_med", Name="Adept QTE", Description="Home Row Double-Time", Type=RodType.Qte, Difficulty=QteDifficulty.Medium, UnlockCost=1 },
            new RodDef { Id="rod_qte_hard", Name="Journeyman QTE", Description="Top & Home Row", Type=RodType.Qte, Difficulty=QteDifficulty.Hard, UnlockCost=1 },
            new RodDef { Id="rod_qte_exp", Name="Pro QTE", Description="Letters & Numbers", Type=RodType.Qte, Difficulty=QteDifficulty.Expert, UnlockCost=1 },
            new RodDef { Id="rod_ascii", Name="Hacker Rod", Description="Type Words", Type=RodType.Ascii, UnlockCost=1 },
            new RodDef { Id="rod_rhythm", Name="Disco Rod", Description="DDR Arrows", Type=RodType.Rhythm, UnlockCost=1 }
        };
        
        // NEW: Sort the rods by unlock cost to ensure progression
        AvailableRods = AvailableRods.OrderBy(r => r.UnlockCost).ToList();
        
        ActiveRod = AvailableRods[0];

        _fishEcosystem = new Dictionary<RodType, List<FishDef>>
        {
            { RodType.Basic, new List<FishDef> { 
                new FishDef { Name="Minnow", MinLength=1, MaxLength=5, Rarity=1.0 },
                new FishDef { Name="Perch", MinLength=8, MaxLength=25, Rarity=0.8 }
            }},
            { RodType.Qte, new List<FishDef> { 
                new FishDef { Name="Salmon", MinLength=40, MaxLength=90, Rarity=1.0 },
                new FishDef { Name="Swordfish", MinLength=100, MaxLength=250, Rarity=0.4 } 
            }},
            { RodType.Ascii, new List<FishDef> { new FishDef { Name="GlitchShark", MinLength=100, MaxLength=300, Rarity=1.0 } } },
            { RodType.Rhythm, new List<FishDef> { new FishDef { Name="DiscoTrout", MinLength=15, MaxLength=45, Rarity=1.0 } } }
        };
    }

     public List<string> GetAllSpeciesNames()
    {
        var list = new List<string>();
        foreach (var pool in _fishEcosystem.Values)
        {
            list.AddRange(pool.Select(f => f.Name));
        }
        return list.Distinct().ToList();
    }

    private async Task SaveAsync()
    {
        var data = new FishingSaveData
        {
            TotalFish = TotalFishCaught,
            PersonalBests = PersonalBests,
            UnlockedRodIds = AvailableRods.Where(r => r.IsUnlocked).Select(r => r.Id).ToList(),
            ActiveRodId = ActiveRod?.Id ?? "rod_1"
        };

        // Note: Using fire-and-forget or awaiting depending on method signature
        await _localStorage.SetItemAsync(SaveKey, data);
    }

    public async Task LoadAsync()
    {
        if (await _localStorage.ContainKeyAsync(SaveKey))
        {
            var data = await _localStorage.GetItemAsync<FishingSaveData>(SaveKey);
            if (data != null)
            {
                TotalFishCaught = data.TotalFish;
                PersonalBests = data.PersonalBests ?? new();
                foreach (var rod in AvailableRods)
                {
                    rod.IsUnlocked = data.UnlockedRodIds.Contains(rod.Id);
                }
                ActiveRod = AvailableRods.FirstOrDefault(r => r.Id == data.ActiveRodId) ?? AvailableRods[0];
                NotifyStateChanged();
            }
        }
    }
    
    public async Task<CaughtFish> GenerateCatch()
    {
        TotalFishCaught++;
        var rng = new Random();
        var possibleFish = _fishEcosystem[ActiveRod.Type];
        var caughtDef = possibleFish
            .OrderBy(f => rng.NextDouble() * f.Rarity) // Randomize based on rarity weight
            .First();

        double range = caughtDef.MaxLength - caughtDef.MinLength;
        double length = caughtDef.MinLength + (range * ((new Random().NextDouble() + new Random().NextDouble()) / 2.0));

        var catchData = new CaughtFish { Name = caughtDef.Name, Length = length, SpriteUrl = caughtDef.SpriteUrl };
        
        // NEW: Update the player's personal records
        UpdatePersonalBests(catchData);
        await SaveAsync();
        NotifyStateChanged();
        return catchData;
    }

    private void UpdatePersonalBests(CaughtFish fish)
    {
        if (PersonalBests.TryGetValue(fish.Name, out var record))
        {
            record.Count++;
            if (fish.Length > record.MaxLength)
            {
                record.MaxLength = fish.Length;
            }
        }
        else
        {
            PersonalBests[fish.Name] = new PlayerFishRecord { Name = fish.Name, Count = 1, MaxLength = fish.Length };
        }
    }

    public async Task UnlockRod(RodDef rod)
    {
        if (TotalFishCaught >= rod.UnlockCost && !rod.IsUnlocked)
        {
            TotalFishCaught -= rod.UnlockCost;
            rod.IsUnlocked = true;
            ActiveRod = rod;
            await SaveAsync();
            NotifyStateChanged();
        }
    }
    
    public async Task EquipRod(RodDef rod)
    {
        if (rod.IsUnlocked) ActiveRod = rod;
        await SaveAsync();
        NotifyStateChanged();
    }

    public FishingSaveData GetSaveState()
    {
        return new FishingSaveData
        {
            TotalFish = TotalFishCaught,
            PersonalBests = PersonalBests,
            UnlockedRodIds = AvailableRods.Where(r => r.IsUnlocked).Select(r => r.Id).ToList(),
            ActiveRodId = ActiveRod?.Id ?? "rod_1"
        };
    }

    public void RestoreSaveState(FishingSaveData data)
    {
        if (data == null) return;

        TotalFishCaught = data.TotalFish;
        PersonalBests = data.PersonalBests ?? new();

        // Re-unlock rods based on the saved IDs
        foreach (var rod in AvailableRods)
        {
            if (data.UnlockedRodIds.Contains(rod.Id)) 
            {
                rod.IsUnlocked = true;
            }
        }

        // Restore the active rod
        var savedActiveRod = AvailableRods.FirstOrDefault(r => r.Id == data.ActiveRodId);
        if (savedActiveRod != null)
        {
            ActiveRod = savedActiveRod;
        }

        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}