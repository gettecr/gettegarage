using MudBlazor;

namespace GetteGarage.Client.Data;

public class StudioProject
{
    public string Title { get; set; } = "";
    public string Version { get; set; } = "";
    public string StaticImage { get; set; } = "";
    public string AnimatedGif { get; set; } = "";
    public List<Tag> Tags { get; set; } = new();
    public string Description { get; set; } = "";
    public string TechStack { get; set; } = "";
    public string StoryPath  { get; set; } = "";
}

public class Tag : IEquatable<Tag>
{
	public string Name {get; }
	public string HexColor {get; }
	public string TextColor {get;}

	public Tag(string name)
	{
		Name = name;
		HexColor = TagData.GetHex(name);
        	TextColor = TagData.GetTextColor(HexColor);
    	}

    	public bool Equals(Tag? other) =>
        	other is not null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    	public override bool Equals(object? obj) => Equals(obj as Tag);
    	public override int GetHashCode() => Name.ToUpperInvariant().GetHashCode();
    	public override string ToString() => Name;
}

public static class TagData
{
	private static readonly Dictionary<string, string> HexColors = new(StringComparer.OrdinalIgnoreCase)
	{
		["Video Game"] = "#f6a91f", // orange (Primary)
		["Web"]        = "#cf4024", // burnt red (Secondary)
		["Art"]        = "#2ecc71", // green (Tertiary)
		["Tool"]       = "#00d9ff", // cyan
		["Web"]     = "#e91e8c", // magenta
		["Misc"]     = "#f4d03f", // yellow	
	};
	private const string DefaultHex = "#888888";

	public static string GetHex(string tag)
    	{
        	if (HexColors.TryGetValue(tag, out var hex))
            		return hex;

        	Console.WriteLine($"[Warning] Tag \"{tag}\" has no registered color — using default.");
        	return DefaultHex;
    	}

    	// Picks black or white text based on background brightness, so a
    	// pale color like the yellow doesn't end up with unreadable white text.
    	public static string GetTextColor(string hex)
	{
		hex = hex.TrimStart('#');
		int r = Convert.ToInt32(hex[..2], 16);
		int g = Convert.ToInt32(hex[2..4], 16);
		int b = Convert.ToInt32(hex[4..6], 16);
		var luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
		return luminance > 0.7 ? "#0d0e1b" : "#ffffff";
	}
}



