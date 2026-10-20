using MudBlazor;

namespace GetteGarage.Client.Data;

public static class StudioProjectData
{
	public static List<StudioProject> All {get; } = BuildProjects();

	private static List<StudioProject> BuildProjects()
	{
		var projects = new List<StudioProject>
		{	
			new StudioProject
			{
				Title="Gette Garage", 
				Version="v1.0",
				Tags= new() {new Tag("Web")},
				Description="This website. Full-stack .NET web app.",
				TechStack=".NET 8, Azure",
				StaticImage="/art/gettegarage.png",
				StoryPath="/stories/gette_garage.md",
			},
			new StudioProject
			{
				Title="For The Birds",
				Version="ALPHA 0.3",
				Tags= new() {new Tag("Video Game")},
				Description="Turn an abandoned golf course into a thriving bird sanctuary. Plant native habitat, attract birds, and photograph them. Think Stardew Valley meets Pokémon Snap.",
				TechStack="Godot 4",
				StaticImage="/art/forthebirds.png",
				AnimatedGif="/art/forthebirds.gif",
				StoryPath="/stories/for_the_birds.md",
			},
			new StudioProject 
			{
				Title = "Pixel art -- OPM", 
				Tags= new() {new Tag("Art")},
				StaticImage = "art/opm_pixel_art.png",
				Gallery = new()
				{
					new GalleryImage {Src = "/art/wrangler.png", Caption= "Team image I made for the 'Data Wranglers'"},
					new GalleryImage {Src="/art/badger.png", Caption= "Badger Jan"},
		
					new GalleryImage {Src="/art/digit.png", Caption= "Digit (DGT)"},
					new GalleryImage {Src="/art/bison.png", Caption= "Design System Dakota"},
					new GalleryImage {Src="/art/GPPApotamus_big.png", Caption= "GPPApotamus"},
					new GalleryImage {Src="/art/rexi_spring.png", Caption= "Rexi (Spring)"},
					new GalleryImage {Src="/art/rexi_summer.png", Caption= "Rexi (Summer)"},
					new GalleryImage {Src="/art/rexi_fall.png", Caption= "Rexi (Fall)"},
					new GalleryImage {Src="/art/rexi_winter.png", Caption= "Rexi (Winter)"},
					new GalleryImage {Src="/art/rexi_prototype.png", Caption= "Rexi (Prototype)"},
					new GalleryImage {Src="/art/rexi_research.png", Caption= "Rexi (Research)"},
					new GalleryImage {Src="/art/rexi_cry.png", Caption= "Rexi (Cry)"},
					new GalleryImage {Src="/art/rexi_hammer.png", Caption= "Rexi (Hammer Pants)"},
					new GalleryImage {Src="/art/rexi_newyear.png", Caption= "Rexi (New Year)"},
				},
				Description="Various pixel art mascots I made for our teams at OPM.",
				TechStack = "Aesprite"
			},
			new StudioProject
			{
				Title="Big Larry's Beach Baseball",
				Version="v1.0",
				Tags= new() {new Tag("Video Game")},
				Description="'Shooting gallery' gameplay with batting cage timing and Larry. What's not to love?",
				TechStack="Godot, WebGL",
				StaticImage="/art/big_larrys_thumbnail.png",
				AnimatedGif="/art/big-larry-card.gif",
				StoryPath = "/stories/big_larrys.md",
			},
			new StudioProject
			{
				Title = "Birdsong to MIDI",
				Tags = new() {new Tag("Tool")},
				StaticImage="",
				TechStack="Godot, Drawing pad, FLStudio, Python",
			},
			new StudioProject 
			{
				Title = "D&D Character Portraits",
				Tags= new() {new Tag("Art")},
				StaticImage="/art/trading_cards_argon.png", 
				AnimatedGif= "/art/trading_cards.png", 
				Description = "Trading card character portraits. Oil on card stock 2.5\"x3.5\" ea.",
				Gallery = new()
				{
					new GalleryImage {Src="/art/art_smart.png", Caption= "Art"},
					new GalleryImage {Src="/art/thalmor.png", Caption= "Thalmor"},
					new GalleryImage {Src="/art/chanel.png", Caption= "Chanel"},
					new GalleryImage {Src="/art/dorgothy.png", Caption= "Dorgothy"},
					new GalleryImage {Src="/art/argon.png", Caption= "Argon"},
					new GalleryImage {Src="/art/erdric.png", Caption= "Erdric"},
				},
				TechStack = "Oil on card stock"
			},
			new StudioProject
			{
				Title = "Local Library Genre Analysis",
				Tags = new() {new Tag("Data")},
				StaticImage="",
				TechStack="Python, Gemini",
			},
			new StudioProject
			{
				Title="Fish Quick",
				Version="v1.0",
				Tags= new() {new Tag("Video Game")},
				Description="A Cast lines and catch fish type game.",
				TechStack="Blazor WASM",
				StaticImage="/art/FishQuick.png",
			},
			new StudioProject
			{
				Title = "KDE Power Inhibitor",
				Tags = new() {new Tag("Tool")},
				StaticImage = "",
				TechStack = "Bash, Manjaro Linux",
			},
			new StudioProject
			{
				Title="Trash Bandit",
				Version="CONCEPT",
				Tags= new() {new Tag("Video Game")},
				Description="Raccoon jump good in a DK '94 style platformer",
				TechStack="Godot 4",
				StaticImage="/art/trashbandit.png",
				AnimatedGif="/art/trashbandit.gif",
				StoryPath="/stories/trash_bandit.md",
			},
			new StudioProject
			{
				Title = "Pixel Art Animations", 
				Tags= new() {new Tag("Art")},
				StaticImage = "/art/big_larry_share.png",
				AnimatedGif = "/art/big_larry_share.gif",
				Gallery = new()
				{
					new GalleryImage { Src = "/art/big_larry_share.gif", Caption = "Big Larry"},
					new GalleryImage {Src = "/art/prototype_henry_share.gif", Caption="Henry"},
					new GalleryImage {Src = "/art/trash_bandit_flip.gif", Caption = "Trash Bandit"},
				},
				Description = "Collection of pixel art animations for games.",
				TechStack = "Aesprite",
				StoryPath="/stories/pixel_art_animations.md"
			},
			new StudioProject
			{
				Title = "Figure Drawing and Painting",
				Tags = new() {new Tag("Art")},
				StaticImage="",
				Gallery = new()
				{
				},
				TechStack = "Mixed Media",

			},
			new StudioProject
			{
				Title="Tidebreaker", 
				Version="v1.2",
				Tags= new() {new Tag("Video Game")},
				Description="Breakout. But with 100% more water.",
				TechStack="Godot, WebGL",
				StaticImage="/art/tidebreaker_thumbnail.png",
				AnimatedGif="/art/tidebreaker-card.gif",
			},
			new StudioProject
			{
				Title = "VineScribe: wine note digitizer",
				Tags = new() {new Tag("Tool")},
				StaticImage="",
				TechStack="Gemini, Gmail, Raspberry Pi, Python",
			},
		};

		foreach (var project in projects.Where(p => string.IsNullOrEmpty(p.StoryPath)))
		{
			Console.WriteLine($"[Warning] StudioProject \"{project.Title}\" has no StoryPath set.");
		}

		return projects;
			
	}
}

