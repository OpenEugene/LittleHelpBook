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
using M= OpenEugene.Module.LittleHelpBook.Models;
using OpenEugene.Module.LittleHelpBook.Client.Viewmodels;
using OpenEugene.Module.LittleHelpBook.Client.Extensions;
using MudBlazor;

namespace OpenEugene.Module.Address;

public partial class Index : ModuleBase
{
    List<M.Address> _list;
		
    [Inject] public AddressService AddressService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IStringLocalizer<Index> Localizer { get; set; }
    [Inject] public ISettingService SettingService { get; set; }
    [Inject] public MudBlazor.IDialogService dialogService { get; set; }

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

    private int _providerId = -1;

    public override string UrlParametersTemplate => Routing.AddressTemplate;


    protected override async Task OnInitializedAsync()
    {
        try
        {
            var moduleSettings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);
            _settingsVM = new SettingsViewModel(SettingService, moduleSettings);
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Loading LittleHelpBook {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!ShouldRender()) return;
        if (!UrlParameters.ContainsKey(Routing.ProviderId)) return;  // route complete?
        if (UrlParameters[Routing.ProviderId] == Routing.Actions.Add) return; // add new provider

        if (UrlParameters.ContainsKey(Routing.ProviderId)) {

            _providerId = int.Parse(UrlParameters[Routing.ProviderId]);

            (_list, var code) = await AddressService.GetAddressesAsync(_providerId);
            if (!this.IsSuccessStatusCode(code))
            {
                throw new HttpRequestException($"Error loading LittleHelpBooks. Code: {code}");
            }

            IsLoaded = true;
        }
    }

    private void Edit(M.Address item)
    {
        var url = this.ComposeUrl(
            basePath: PageState.Page.Path,
            moduleId: ModuleState.ModuleId,
            action: "Edit",
            _providerId, item.AddressId);

        NavigationManager.NavigateTo(url);
    }

    private void Add()
    {
        var url = this.ComposeUrl(
            basePath: PageState.Page.Path,
            moduleId: ModuleState.ModuleId,
            action: "Add",
            _providerId);

        NavigationManager.NavigateTo(url);
    }

    private async Task Delete(M.Address item)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, };
        // confirm delete using MudBlazor Dialog
        var confirm = await dialogService.ShowMessageBox("Delete?",
            $"Delete address {item.Address1}?",
            "Yes", cancelText: "No", options: options);

        if (!confirm.HasValue || !confirm.Value) return;

        try
        {
            var code = await AddressService.DeleteAddressAsync(item.AddressId);
            if (!this.IsSuccessStatusCode(code)) {
                throw new HttpRequestException($"Error Deleting LittleHelpBooks. id:{item.AddressId}, Code: {code}");
            }
            await logger.LogInformation("LittleHelpBook Deleted {item}", item);

            (_list, code ) = await AddressService.GetAddressesAsync(_providerId);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Deleting LittleHelpBook {item} {Error}", item, ex.Message);
            AddModuleMessage(Localizer["Message.DeleteError"], MessageType.Error);
        }
    }

}

