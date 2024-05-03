using MudBlazor;
using Narochno.Primitives;

namespace BottleSplitter.Services;

public interface IThemeService
{
    void SetTheme(MudTheme theme);
    MudTheme? GetTheme();
    void SetThemeProvider(MudThemeProvider themeProvider);

    MudThemeProvider? GetThemeProvider();
}
public class ThemeService : IThemeService
{
    private  MudTheme? _theme;
    private MudThemeProvider? _themeProvider;

    public void SetTheme(MudTheme theme) => _theme = theme;

    public MudTheme? GetTheme() => _theme;
    public void SetThemeProvider(MudThemeProvider themeProvider) => _themeProvider = themeProvider;

    public MudThemeProvider? GetThemeProvider() => _themeProvider;
}
