using MudBlazor;

namespace GetteGarage.Client.Data;

public static class AppTheme
{
    public static MudTheme GameTheme { get; } = CreateGameTheme();

    private static MudTheme CreateGameTheme()
    {
        var theme = new MudTheme();

        theme.PaletteDark = new PaletteDark()
        {
            Primary = "#f6a91f",
            Secondary = "#cf4024",
            Success = "#f6a91f",
            Warning = "#f6a91f",
            Background = "#121324",
            Surface = "#2d2e45",
            AppbarBackground = "#000000",
            TextPrimary = "#e0e0e0",
            Tertiary = "#2ecc71",
            DrawerBackground = "#2d2e45",
            ActionDefault = "#f6a91f"
        };

        var hudFont = new[] { "Rajdhani", "sans-serif" };
        var pixelFont = new[] { "Press Start 2P", "cursive" };

        theme.Typography.Default.FontFamily = hudFont;
        theme.Typography.H1.FontFamily = pixelFont;
        theme.Typography.H1.LineHeight = "1.5";
        theme.Typography.H2.FontFamily = pixelFont;
        theme.Typography.H2.LineHeight = "1.5";
        theme.Typography.H3.FontFamily = pixelFont;
        theme.Typography.H3.LineHeight = "1.5";
        theme.Typography.H4.FontFamily = pixelFont;
        theme.Typography.H4.LineHeight = "1.5";
        theme.Typography.H5.FontFamily = pixelFont;
        theme.Typography.H5.LineHeight = "1.5";
        theme.Typography.H6.FontFamily = pixelFont;
        theme.Typography.H6.LineHeight = "1.5";
        theme.Typography.Button.FontFamily = pixelFont;
        theme.Typography.Button.FontSize = ".7rem";

        return theme;
    }
}
