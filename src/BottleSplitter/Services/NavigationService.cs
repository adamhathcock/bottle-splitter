using Microsoft.AspNetCore.Components;
using Speckle.InterfaceGenerator;

namespace BottleSplitter.Services;

[GenerateAutoInterface]
public class NavigationService(NavigationManager navigationManager) : INavigationService
{
    public void GoToIndex(bool refresh = false) => navigationManager.NavigateTo("/", refresh);

    public void GoToSplits(bool refresh = false) =>
        navigationManager.NavigateTo("/splits", refresh);

    public void GoTo(string url, bool refresh = false) =>
        navigationManager.NavigateTo(url, refresh);
}
