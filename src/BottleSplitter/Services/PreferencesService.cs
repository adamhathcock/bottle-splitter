using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using Narochno.Primitives;
using Speckle.InterfaceGenerator;

namespace BottleSplitter.Services;

public class Preferences
{
    public bool DarkMode { get; set; }
}

[GenerateAutoInterface]
public class PreferencesService(
    ProtectedLocalStorage protectedLocalStorage,
    ILogger<PreferencesService> logger
) : IPreferencesService
{
    private Preferences _preferences = new();
    public bool DarkMode
    {
        get => _preferences.DarkMode;
        set => _preferences.DarkMode = value;
    }

    public async Task Save()
    {
        await protectedLocalStorage.SetAsync("bottle-splitter-prefs", _preferences);
    }

    public async Task Load()
    {
        var val = await protectedLocalStorage.GetAsync<Preferences>("bottle-splitter-prefs");
        if (val.Success)
        {
            try
            {
                _preferences = val.Value.NotNull();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Error loading preferences.");
            }
        }
    }
}
