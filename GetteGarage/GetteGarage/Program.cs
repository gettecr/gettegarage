using GetteGarage.Client.Pages;
using GetteGarage.Components;
using MudBlazor.Services;
using GetteGarage.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.ResponseCompression;
using GetteGarage.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BaseAddress"] ?? "http://localhost:5194") });
builder.Services.AddScoped<BlogService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<GetteGarage.Client.Services.AchievementService>(); 
builder.Services.AddSingleton<HighScoreService>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();

//  mappings for Godot files
provider.Mappings[".pck"] = "application/octet-stream";
provider.Mappings[".wasm"] = "application/wasm";


// use these mappings
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(GetteGarage.Client._Imports).Assembly);

app.MapHub<GarageHub>("/garagehub");

app.Run();

