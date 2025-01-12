using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;
using Oqtane.Services;

using OpenEugene.Module.LittleHelpBook.Services;
using OpenEugene.Module.LittleHelpBook;
using MudBlazor;
using OpenEugene.Module.LittleHelpBook.Shared;
using Oqtane.Security;

namespace OpenEugene.Module.Provider;

public partial class Index : ModuleBase
{
    List<LittleHelpBook.Models.Provider> _list;
    private string _searchString;

    [Inject] public ProviderService ProviderService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IStringLocalizer<Index> Localizer { get; set; }
    [Inject] public ISettingService SettingService { get; set; }
	
    public override List<Resource> Resources => new List<Resource>()
    {
        new Resource { ResourceType = ResourceType.Stylesheet,  Url = "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" },
        new Resource { ResourceType = ResourceType.Stylesheet,  Url = "_content/MudBlazor/MudBlazor.min.css" },
        new Resource { ResourceType = ResourceType.Stylesheet,  Url = ModulePath() + "Module.css" },
        new Resource { ResourceType = ResourceType.Script,      Url = "_content/MudBlazor/MudBlazor.min.js", Location = ResourceLocation.Body, Level = ResourceLevel.Site },
        new Resource { ResourceType = ResourceType.Script,      Url = ModulePath() + "Module.js" },
    };	
    private bool IsLoaded;
    private SettingsViewModel _settingsVM; 

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var moduleSettings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);
            _settingsVM = new SettingsViewModel(SettingService, moduleSettings);
          
            (_list, var code) = await ProviderService.GetProvidersAsync();
            if (!IsSuccessStatusCode(code)) {
                throw new HttpRequestException($"Error loading LittleHelpBooks. Code: {code}");
            }

            IsLoaded = true;
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Loading LittleHelpBook {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
        }
    }

    private Func<LittleHelpBook.Models.Provider, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (!string.IsNullOrWhiteSpace(x.Description)
         && x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (!string.IsNullOrWhiteSpace(x.Name)
         && x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    private async Task Delete(LittleHelpBook.Models.Provider item)
    {
        try
        {
            await ProviderService.DeleteProviderAsync(item.ProviderId);
            await logger.LogInformation("Provider Deleted {item}", item);
            (_list, var code) = await ProviderService.GetProvidersAsync();
            if (!IsSuccessStatusCode(code))
            {
                throw new HttpRequestException($"Error loading LittleHelpBooks. Code: {code}");
            }
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Deleting Provider {item} {Error}", item, ex.Message);
            AddModuleMessage(Localizer["Message.DeleteError"], MessageType.Error);
        }
    }

    private void Selected(DataGridRowClickEventArgs<LittleHelpBook.Models.Provider> args)
    {
        // check the asp.net roles to see where to navigate
        if (UserSecurity.IsAuthorized(PageState.User, LhbRoleNames.Editors))
        {
            Edit(args.Item);
        }
        else
        {
            Detail(args.Item);
        }
    }

    private void Edit(LittleHelpBook.Models.Provider item)
    {
        var url = EditUrl("Edit", $"id={item.ProviderId}");
        NavigationManager.NavigateTo(url);
    }

    private void Detail(LittleHelpBook.Models.Provider item)
    {
        var url = EditUrl("Detail", $"id={item.ProviderId}");
        NavigationManager.NavigateTo(url);
    }

    private void Add()
    {
        var url = EditUrl("Add");
        NavigationManager.NavigateTo(url);
    }


    static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
        return (int)statusCode >= 200 && (int)statusCode <= 299; 
    }
}

