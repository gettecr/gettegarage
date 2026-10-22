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
				TechStack = "Aesprite",
				StoryPath = "stories/opm_pixel_art_mascots.md"
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
				Tags = new() {new Tag("Tool"), new Tag("Art")},
				StaticImage="/art/sonotrace.png",
				AnimatedGif="/art/sonotrace.gif",
				Description="An instrument developed to use a Wacom drawing pad-to-MIDI-to-synth workflow",
				TechStack="Godot, Wacom Drawing pad, FLStudio, Python",
				StoryPath="/stories/birdsong_midi.md"
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
				TechStack = "Oil on card stock",
				StoryPath = "stories/dnd_characters.md"
			},
			new StudioProject
			{
				Title = "Local Library Genre Analysis",
				Tags = new() {new Tag("Data"), new Tag("AI")},
				StaticImage="/art/proportion_circ_ya_edit.png",
				Description="An exploratory analysis of a local library's collection, plus an AI-labeled genre breakdown of its fiction section. Which books actually get borrowed?",
				TechStack="Python, Gemini, Pandas",
				StoryPath="/stories/library_analysis.md"
			},
			new StudioProject
			{
				Title="Fish Quick",
				Version="v1.0",
				Tags= new() {new Tag("Video Game")},
				Description="A fishing game built from quick-time events and keyboard minigames, with emojis standing in for art.",
				TechStack="Blazor WASM",
				StaticImage="/art/FishQuick.png",
				StoryPath="/stories/fish_quick.md"
			},
			new StudioProject
			{
				Title = "KDE Power Inhibitor",
				Tags = new() {new Tag("Tool")},
				StaticImage = "/art/kde_inhibitor.png",
				Description="A tiny set of Linux scripts that keeps your laptop awake while music plays or a DAW is open, without keeping the screen on.",
				TechStack = "Bash, Manjaro Linux",
				StoryPath="/stories/kde_power_inhibitor.md"
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
				StaticImage="/art/portrait_2.jpg",
				Gallery = new()
				{
					new GalleryImage {Src = "/art/gouache_2.jpg", Caption= "Gouache on toned sketchbook"},
					new GalleryImage {Src="/art/oil_figures.jpg", Caption= "Oil on canvas"},
					new GalleryImage {Src="/art/portrait_2.jpg", Caption= "Charcoal on paper"},
					new GalleryImage {Src="/art/firefighter.jpg", Caption= "Colored pencil and ink"},
					new GalleryImage {Src="/art/dancer.jpg", Caption= "Colored ink"},
					new GalleryImage {Src="/art/baseball_1.jpg", Caption= "Colored pencil"},
					new GalleryImage {Src="/art/baseball_2.jpg", Caption= "Charcoal"},
					new GalleryImage {Src="/art/basketball_1.jpg", Caption= "Colored pencil"},
					new GalleryImage {Src="/art/basketball_2.jpg", Caption= "Colored pencil and ink"},
					new GalleryImage {Src="/art/fig_1.png", Caption= "Graphite"},
					new GalleryImage {Src="/art/gestures.jpg", Caption= "Charcoal"},
					new GalleryImage {Src="/art/gouache_1.jpg", Caption= "Gouache on toned paper"},
					new GalleryImage {Src="/art/muaythai.jpg", Caption= "Colored pencil and graphite"},
					new GalleryImage {Src="/art/muaythai_2.jpg", Caption= "Colored pencil and ink"},
					new GalleryImage {Src="/art/portrait_1.jpg", Caption= "Oil on canvas"},
					new GalleryImage {Src="/art/smoking.jpg", Caption= "Graphite"},
					new GalleryImage {Src="/art/wizard.jpg", Caption= "Colored pencil and ink"},
				},
				Description="Sketchbooks, box drills, and oil portraits. Practice work, not finished products.",
				TechStack = "Mixed Media",
				StoryPath = "/stories/figure_drawing.md"

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
				StoryPath="/stories/tidebreaker.md",
			},
			new StudioProject
			{
				Title = "VineScribe: wine note digitizer",
				Tags = new() {new Tag("Tool"), new Tag("AI")},
				StaticImage="/art/wine_notebook.jpg",
				Description = "Email a photo of a handwritten wine tasting note and a Raspberry Pi digitizes it. Email a wine label and it tells you if you've tried it before.",
				TechStack="Gemini, Gmail, Raspberry Pi, Python",
				StoryPath="/stories/vinescribe.md"
			},
		};

		foreach (var project in projects.Where(p => string.IsNullOrEmpty(p.StoryPath)))
		{
			Console.WriteLine($"[Warning] StudioProject \"{project.Title}\" has no StoryPath set.");
		}

		return projects;
			
	}
}

