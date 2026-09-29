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
	public Color TagColor {get; }

	public Tag(string name)
	{
		Name = name;
		TagColor = TagData.GetColor(name);
	}

	public bool Equals(Tag? other) =>
		other is not null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

	public override bool Equals(object? obj) => Equals(obj as Tag);

	public override int GetHashCode() =>
		Name.ToUpperInvariant().GetHashCode();

	public override string ToString() => Name;
}

public static class TagData
{
	private static readonly Dictionary<string, Color> Colors = new(StringComparer.OrdinalIgnoreCase)
	{
		["Video Game"] = Color.Warning,
		["Web"] = Color.Info,
		["Art"] = Color.Tertiary,
		["Analysis"] = Color.Secondary,
	};

	private static readonly Color DefaultColor = Color.Default;

	public static Color GetColor(string tag) =>
		Colors.TryGetValue(tag, out var color) ? color : DefaultColor;
}



