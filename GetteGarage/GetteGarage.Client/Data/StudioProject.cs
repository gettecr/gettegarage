using MudBlazor;

namespace GetteGarage.Client.Data;

public class StudioProject
{
    public string Title { get; set; } = "";
    public string Version { get; set; } = "";
    public string StaticImage { get; set; } = "";
    public string AnimatedGif { get; set; } = "";
    public string Tag { get; set; } = "";
    public Color TagColor { get; set; }
    public string Description { get; set; } = "";
    public string TechStack { get; set; } = "";
    public string StoryPath  { get; set; } = "";
}

