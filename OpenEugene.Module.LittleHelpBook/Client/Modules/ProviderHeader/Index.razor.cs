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
using OpenEugene.Module.LittleHelpBook.ViewModels;
using Oqtane.UI;
using OpenEugene.Module.LittleHelpBook.Client.Extensions;
using MudBlazor;
using OpenEugene.Module.Client.Controls;
using static MudBlazor.CategoryTypes;

namespace OpenEugene.Module.ProviderHeader;

public partial class Index : ModuleBase
{
    M.Provider _model;
		
    [Inject] public ProviderService ProviderService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IStringLocalizer<Index> Localizer { get; set; }
    [Inject] public ISettingService SettingService { get; set; }
    [Inject] IDialogService DialogService { get; set; }

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
    public override string Title => "Provider";


    private int _providerId = -1;

    public override string UrlParametersTemplate => Routing.ProviderTemplate;


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

        if (UrlParameters[Routing.ProviderId] == Routing.Actions.Add) {
                
            var url = this.ComposeUrl(
                basePath: PageState.Page.Path,
                moduleId: ModuleState.ModuleId,
                action: "Add",
                0);

            NavigationManager.NavigateTo(url);
            return;
        }

        _providerId = int.Parse(UrlParameters[Routing.ProviderId]);

        (_model, var code) = await ProviderService.GetProviderAsync(_providerId);
        if (!IsSuccessStatusCode(code))
        {
            throw new HttpRequestException($"Error loading Providers. Code: {code}");
        }

        IsLoaded = true;
    }

    private void Back() { 
        NavigationManager.NavigateTo(Routing.ProviderList);
    }

    private async Task Delete()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, };
        // confirm delete using MudBlazor Dialog
        var confirm = await DialogService.ShowMessageBox("Delete?",
            $"Delete Provider {_model.Name}?",
            "Yes", cancelText: "No", options: options);

        if (!confirm.HasValue || !confirm.Value) return;

        try
        {
            var code = await ProviderService.DeleteProviderAsync(_model.ProviderId);
            if (code is not HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Error Deleting {_model}. Code: {code}");
            }
            await logger.LogInformation("LittleHelpBook Deleted {_item}", _model);
            NavigationManager.NavigateTo(Routing.ProviderList);
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Deleting LittleHelpBook {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.DeleteError"], MessageType.Error);
        }
     
    }


    private void Edit()
    {
        var url = this.ComposeUrl(
            basePath: PageState.Page.Path,
            moduleId: ModuleState.ModuleId,
            action: "Edit",
            _providerId);

        NavigationManager.NavigateTo(url);
    }


    static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
        return (int)statusCode >= 200 && (int)statusCode <= 299; 
    }
}

