using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 1. Register MudBlazor
// This is required for MudGrid, MudCard, and IDialogService (Art Portfolio)
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage(); 

// 2. Register HttpClient
// This allows your Client components to fetch JSON files or assets 
// from your wwwroot folder if you need to later.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GetteGarage.Client.Services.AchievementService>();
builder.Services.AddScoped<CodeGameEngine>();
await builder.Build().RunAsync();
