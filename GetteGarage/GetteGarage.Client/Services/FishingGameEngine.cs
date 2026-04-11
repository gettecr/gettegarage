using GetteGarage.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace GetteGarage.Client.Services
{
    public class FishingGameEngine
    {
        private readonly ILocalStorageService _localStorage;
        
        private const string SaveKey = "fish_save_v2"; 
        
        public int TotalFishCaught { get; private set; } = 0;
        public double TotalMoney { get; private set; } = 0.0;
        
        public List<RodDef> AvailableRods { get; private set; } = new();
        public RodDef ActiveRod { get; private set; }
        public VisualTheme CurrentTheme { get; private set; } = new();

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
                new RodDef { Id="rod_qte_easy", Name="Novice QTE", Description="Home Row Keys", Type=RodType.Qte, Difficulty=QteDifficulty.Easy, UnlockCost=50 },
                new RodDef { Id="rod_ascii", Name="Hacker Rod", Description="Type Words", Type=RodType.Ascii, UnlockCost=250 },
                new RodDef { Id="rod_qte_med", Name="Adept QTE", Description="Home Row Double-Time", Type=RodType.Qte, Difficulty=QteDifficulty.Medium, UnlockCost=1000 },
                new RodDef { Id="rod_rhythm", Name="Disco Rod", Description="DDR Arrows", Type=RodType.Rhythm, UnlockCost=5000 },
                new RodDef { Id="rod_qte_hard", Name="Journeyman QTE", Description="Top & Home Row", Type=RodType.Qte, Difficulty=QteDifficulty.Hard, UnlockCost=15000 },
                new RodDef { Id="rod_qte_exp", Name="Pro QTE", Description="Letters & Numbers", Type=RodType.Qte, Difficulty=QteDifficulty.Expert, UnlockCost=50000 }
            };
            
            AvailableRods = AvailableRods.OrderBy(r => r.UnlockCost).ToList();
            ActiveRod = AvailableRods[0];

            _fishEcosystem = new Dictionary<RodType, List<FishDef>>
            {
                { RodType.Basic, new List<FishDef> { 
                    new FishDef { Name="Old Boot", Emoji="👢", MinLength=20, MaxLength=40, Rarity=0.8, BaseValue=1 },
                    new FishDef { Name="Minnow", Emoji="🐟", MinLength=1, MaxLength=5, Rarity=1.0, BaseValue=5 },
                    new FishDef { Name="Perch", Emoji="🐠", MinLength=8, MaxLength=25, Rarity=0.5, BaseValue=15 }
                }},
                { RodType.Qte, new List<FishDef> { 
                    // This acts as the QteDifficulty.Easy pool
                    new FishDef { Name="Bass", Emoji="🐟", MinLength=20, MaxLength=60, Rarity=1.0, BaseValue=30 },
                    new FishDef { Name="Trout", Emoji="🐡", MinLength=30, MaxLength=80, Rarity=0.6, BaseValue=50 },
                    new FishDef { Name="Catfish", Emoji="🦈", MinLength=50, MaxLength=150, Rarity=0.2, BaseValue=100 }
                }},
                { RodType.Ascii, new List<FishDef> { 
                    new FishDef { Name="Floppy Disk", Emoji="💾", MinLength=5, MaxLength=15, Rarity=1.0, BaseValue=150 },
                    new FishDef { Name="CyberRay", Emoji="⚡", MinLength=50, MaxLength=120, Rarity=0.5, BaseValue=400 },
                    new FishDef { Name="GlitchShark", Emoji="🦈", MinLength=150, MaxLength=400, Rarity=0.1, BaseValue=1000 } 
                }},
                { RodType.Rhythm, new List<FishDef> { 
                    new FishDef { Name="NeonTetra", Emoji="🐠", MinLength=2, MaxLength=10, Rarity=1.0, BaseValue=1200 },
                    new FishDef { Name="BassDrop", Emoji="🎸", MinLength=80, MaxLength=200, Rarity=0.5, BaseValue=3000 },
                    new FishDef { Name="DiscoTrout", Emoji="🪩", MinLength=100, MaxLength=300, Rarity=0.05, BaseValue=8000 } 
                }}
            };

            // Expanded QTE Ecosystems mapped to fake RodType IDs for storage
            _fishEcosystem.Add((RodType)991 /* Medium */, new List<FishDef> {
                new FishDef { Name="Pike", Emoji="🐟", MinLength=40, MaxLength=130, Rarity=1.0, BaseValue=250 },
                new FishDef { Name="Sturgeon", Emoji="🦈", MinLength=100, MaxLength=300, Rarity=0.4, BaseValue=600 },
                new FishDef { Name="Alligator Gar", Emoji="🐊", MinLength=150, MaxLength=350, Rarity=0.1, BaseValue=1500 }
            });

            _fishEcosystem.Add((RodType)992 /* Hard */, new List<FishDef> {
                new FishDef { Name="Tuna", Emoji="🐟", MinLength=80, MaxLength=250, Rarity=1.0, BaseValue=2500 },
                new FishDef { Name="Mahi-Mahi", Emoji="🐠", MinLength=90, MaxLength=200, Rarity=0.5, BaseValue=6000 },
                new FishDef { Name="Swordfish", Emoji="🗡️", MinLength=150, MaxLength=450, Rarity=0.1, BaseValue=12000 }
            });

            _fishEcosystem.Add((RodType)993 /* Expert */, new List<FishDef> {
                new FishDef { Name="Marlin", Emoji="🐟", MinLength=200, MaxLength=500, Rarity=1.0, BaseValue=15000 },
                new FishDef { Name="Sailfish", Emoji="🐠", MinLength=150, MaxLength=350, Rarity=0.5, BaseValue=35000 },
                new FishDef { Name="Kraken", Emoji="🦑", MinLength=500, MaxLength=2000, Rarity=0.01, BaseValue=100000 } 
            });
        }

        public List<string> GetAllSpeciesNames()
        {
            var list = new List<string>();
            foreach (var pool in _fishEcosystem.Values)
            {
                list.AddRange(pool.Select(f => f.Name));
            }
            return list.Distinct().OrderBy(n => n).ToList();
        }

        public async Task<CaughtFish> GenerateCatch()
        {
            TotalFishCaught++;
            
            // 1. Find the correct Fish Pool based on Rod Type AND Difficulty
            var poolKey = ActiveRod.Type;
            if (ActiveRod.Type == RodType.Qte)
            {
                if (ActiveRod.Difficulty == QteDifficulty.Medium) poolKey = (RodType)991;
                else if (ActiveRod.Difficulty == QteDifficulty.Hard) poolKey = (RodType)992;
                else if (ActiveRod.Difficulty == QteDifficulty.Expert) poolKey = (RodType)993;
            }

            var possibleFish = _fishEcosystem.ContainsKey(poolKey) ? _fishEcosystem[poolKey] : _fishEcosystem[RodType.Basic];
            
            var rng = new Random();
            
            // 2. Rarity Roll (Lower weight means rarer fish)
            var caughtDef = possibleFish
                .OrderByDescending(f => rng.NextDouble() * f.Rarity)
                .First();

            // 3. Size Roll (Bell curve logic you provided)
            double range = caughtDef.MaxLength - caughtDef.MinLength;
            double length = caughtDef.MinLength + (range * ((rng.NextDouble() + rng.NextDouble()) / 2.0));
            length = Math.Round(length, 2);

            // 4. Value Calculation: Base Value * (How close to max size they got)
            double sizeModifier = 1.0 + ((length - caughtDef.MinLength) / (caughtDef.MaxLength - caughtDef.MinLength));
            double value = Math.Round(caughtDef.BaseValue * sizeModifier, 2);
            
            TotalMoney += value;

            var catchData = new CaughtFish 
            { 
                Name = caughtDef.Name, 
                Emoji = caughtDef.Emoji, // Added Emoji support
                Length = length, 
                Value = value,           // Added Value support
                SpriteUrl = caughtDef.SpriteUrl 
            };
            
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
            // Now checks against TotalMoney instead of FishCaught
            if (TotalMoney >= rod.UnlockCost && !rod.IsUnlocked)
            {
                TotalMoney -= rod.UnlockCost;
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

        private async Task SaveAsync()
        {
            var data = new FishingSaveData
            {
                TotalFish = TotalFishCaught,
                TotalMoney = TotalMoney, 
                PersonalBests = PersonalBests,
                UnlockedRodIds = AvailableRods.Where(r => r.IsUnlocked).Select(r => r.Id).ToList(),
                ActiveRodId = ActiveRod?.Id ?? "rod_1"
            };

            await _localStorage.SetItemAsync(SaveKey, data);
        }

        public async Task LoadAsync()
        {
            if (await _localStorage.ContainKeyAsync(SaveKey))
            {
                try 
                {
                    var data = await _localStorage.GetItemAsync<FishingSaveData>(SaveKey);
                    if (data != null)
                    {
                        TotalFishCaught = data.TotalFish;
                        TotalMoney = data.TotalMoney;
                        PersonalBests = data.PersonalBests ?? new();
                        
                        foreach (var rod in AvailableRods)
                        {
                            rod.IsUnlocked = data.UnlockedRodIds.Contains(rod.Id);
                        }
                        
                        ActiveRod = AvailableRods.FirstOrDefault(r => r.Id == data.ActiveRodId) ?? AvailableRods[0];
                    }
                }
                catch { /* Corrupt save, ignore */ }
            }
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}