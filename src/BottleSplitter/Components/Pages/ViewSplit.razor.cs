using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BottleSplitter.Components.Splitter;
using BottleSplitter.Infrastructure;
using BottleSplitter.Model;
using Microsoft.AspNetCore.Components;

namespace BottleSplitter.Components.Pages;

public partial class ViewSplit
{
    [Parameter]
    public string? Squid { get; set; }

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    private EditSplitMembership _editSplitMembership = default!;

    private BottleSplit? Split { get; set; }
    private SplitMembership? Membership { get; set; }
    private ClaimsPrincipal? _currentUser;
    private bool _editMembershipVisible;

    protected override async Task OnInitializedAsync()
    {
        await OnClosingMembership();
        var state = await ServerAuthenticationStateProvider.GetAuthenticationStateAsync();
        _currentUser = state.User;
        Membership = Split?.Members.FirstOrDefault(x => x.User.Id == _currentUser.GetId());
    }

    private void Join()
    {
        _editMembershipVisible = true;
    }

    private void Edit() => NavigationService.GoTo($"/splits/{Squid}/edit");

    private async ValueTask OnClosingMembership()
    {
        if (Squid is null)
        {
            return;
        }
        Split = await SplitManager.GetSplitBySquid(Squid);
    }
}
