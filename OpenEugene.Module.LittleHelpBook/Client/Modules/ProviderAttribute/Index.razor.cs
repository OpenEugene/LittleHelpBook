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
using OpenEugene.Module.LittleHelpBook.Client.Extensions;

namespace OpenEugene.Module.ProviderAttribute;

public partial class Index : ModuleBase
{
    List<ProviderAttributeViewModel> _list;
		
    [Inject] public ProviderAttributeService ProviderAttributeService { get; set; }
    [Inject] public AttributeService AttributeService { get; set; }
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
    public override string Title => "Services";

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

        if (UrlParameters.ContainsKey(Routing.ProviderId)) {

            _providerId = int.Parse(UrlParameters[Routing.ProviderId]);

            (_list, var code) = await ProviderAttributeService.GetAttributesByProviderAsync(_providerId);
            if (!IsSuccessStatusCode(code))
            {
                throw new HttpRequestException($"Error loading LittleHelpBooks. Code: {code}");
            }

            IsLoaded = true;
        }
    }

    private async Task Delete(M.Attribute attribute) {

        await ProviderAttributeService.DeleteAttributeAsync(attribute.AttributeId);
    }

    private async Task Add()
    {
        var url = this.ComposeUrl(
           basePath: PageState.Page.Path,
           moduleId: ModuleState.ModuleId,
           action: "Add",
           _providerId);

        NavigationManager.NavigateTo(url);
    }

    static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
        return (int)statusCode >= 200 && (int)statusCode <= 299; 
    }
}

