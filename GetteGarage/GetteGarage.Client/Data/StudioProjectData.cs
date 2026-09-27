using MudBlazor;

namespace GetteGarage.Client.Data;

public static class StudioProjectData
{
	public static List<StudioProject> All {get; } = new()
	{
		new StudioProject
		{
	 	Title="Big Larry's Beach Baseball",
                Version="v1.0",
		Tag="LIVE",
		TagColor=Color.Tertiary,
                Description="'Shooting gallery' gameplay with batting cage timing and Larry. What's not to love?",
                TechStack="Godot, WebGL",
                StaticImage="/art/big_larrys_thumbnail.png",
                AnimatedGif="/art/big-larry-card.gif",
		},
		new StudioProject
		{
                Title="Tidebreaker", 
                Version="v1.2",
		Tag="LIVE",
		TagColor=Color.Tertiary,
                Description="Breakout. But with 100% more water.",
                TechStack="Godot, WebGL",
                StaticImage="/art/tidebreaker_thumbnail.png",
                AnimatedGif="/art/tidebreaker-card.gif",
		},
		new StudioProject
		{
                Title="Fish Quick",
                Version="v1.0",
		Tag="BUILT IN",
		TagColor=Color.Info,
                Description="A Cast lines and catch fish type game.",
                TechStack="Blazor WASM",
                StaticImage="/art/FishQuick.png",
		},
		new StudioProject
		{
                Title="Gette Garage", 
                Version="v1.0",
		Tag="SYSTEM",
		TagColor=Color.Warning,
                Description="This website. Full-stack .NET web app.",
                TechStack=".NET 8, Azure",
                StaticImage="/art/gettegarage.png",
		},
		new StudioProject
		{

                Title="For The Birds",
                Version="ALPHA 0.3",
		Tag="PROTOTYPE",
		TagColor=Color.Warning,
                Description="Grow your bird sanctuary, attract the birds",
                TechStack="Godot 4",
                StaticImage="/art/forthebirds.png",
                AnimatedGif="/art/forthebirds.gif",
		},
		new StudioProject
		{

                Title="Trash Bandit",
                Version="CONCEPT",
		Tag="R&D",
		TagColor=Color.Secondary,
                Description="Raccoon jump good in a DK '94 style platformer",
                TechStack="Godot 4",
                StaticImage="/art/trashbandit.png",
                AnimatedGif="/art/trashbandit.gif",
		},
		new StudioProject
		{
		Title="Idle Dev Simulator",
                Version="v2.4",
		Tag="COMING SOON",
		TagColor=Color.Info,
                Description="Write code, drink coffee, squash bugs and upgrade your system.",
                TechStack="Blazor WASM",
		},
		new StudioProject
		{
		Title = "Big Larry Animation", 
		Tag= "Pixel Art", 
		StaticImage = "/art/big_larry_share.png",
		AnimatedGif = "/art/big_larry_share.gif", 
		Description = "Big Larry's one-handed swing animation.",
		TechStack = "Aesprite"
		},
        	new StudioProject 
		{ 
		Title = "For The Birds - Henry Animation", 
		Tag = "Pixel Art",
		StaticImage = "/art/prototype_henry_share.png", 
		AnimatedGif = "/art/prototype_henry_share.gif",
		Description = "Run animation for 'Henry'- protagonist of For The Birds.",
		TechStack = "Aesprite"
		},
       	 	new StudioProject 
		{
		Title = "Rexi pixel art -- OPM", 
		Tag= "Pixel Art", 
		StaticImage = "art/opm_pixel_art.png",
		Description="Various versions of Rexi, the chatbot mascot I made for OPM.",
		TechStack = "Aesprite"
		},
        	new StudioProject 
		{
		Title = "The Wranglers -- OPM", 
		Tag="Pixel Art", 
		StaticImage = "art/wrangler.png", 
		Description="Team image I made for the 'Data Wranglers' combining jeeps, jeans, and cats.",
		TechStack = "Aesprite"
		},
        	new StudioProject 
		{
		Title = "D&D Character Portraits",
		Tag = "Traditional Art",
		StaticImage="/art/trading_cards_argon.png", 
		AnimatedGif= "/art/trading_cards.png", 
		Description = "Trading card character portraits. Oil on card stock 2.5\"x3.5\" ea.",
		TechStack = "Oil on card stock"
		},
		
	};
}

