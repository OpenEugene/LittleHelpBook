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
using M = OpenEugene.Module.LittleHelpBook.Models;
using OpenEugene.Module.LittleHelpBook.Client.Viewmodels;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Colors;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OpenEugene.Module.LittleHelpBook.Client.Extensions;
using MudBlazor;

namespace OpenEugene.Module.PhoneNumber;

public partial class Index : ModuleBase
{
    List<LittleHelpBook.Models.PhoneNumber> _list;


    [Inject] public PhoneNumberService PhoneNumberService { get; set; }
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

    public override string UrlParametersTemplate => Routing.PhoneTemplate;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var moduleSettings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);
            _settingsVM = new SettingsViewModel(SettingService, moduleSettings);

        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Loading Phone Number Settings {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
        }
    }
    protected override async Task OnParametersSetAsync()
    {
        if (!ShouldRender()) return;
        if (!UrlParameters.ContainsKey(Routing.ProviderId)) return;  // route complete?

        try
        {
            _providerId = Int32.Parse(UrlParameters[Routing.ProviderId]);
            (_list, var code) = await PhoneNumberService.GetPhoneNumbersAsync(_providerId);
            if (!IsSuccessStatusCode(code))
            {
                throw new HttpRequestException($"Error loading Phone Numbers. Code: {code}");
            }
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Loading Phone Numbers {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
        }
        IsLoaded = true;
    }

    private async Task Delete(M.PhoneNumber phone)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, };
        // confirm delete using MudBlazor Dialog
        var confirm = await dialogService.ShowMessageBox("Delete?",
            $"Delete Phone Number {phone.FullNumber}?",
            "Yes",cancelText:"No",options:options);

        if (!confirm.HasValue || !confirm.Value) return;
        try
        {
            var code = await PhoneNumberService.DeletePhoneNumberAsync(phone.PhoneNumberId);
            if (!IsSuccessStatusCode(code)) {
                throw new HttpRequestException($"Error Deleting LittleHelpBooks. id:{phone.PhoneNumberId}, Code: {code}");
            }
            await logger.LogInformation("Phone Deleted {phone}", phone);

            (_list, code ) = await PhoneNumberService.GetPhoneNumbersAsync(_providerId);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Deleting Phone Number {LittleHelpBook} {Error}", phone, ex.Message);
            AddModuleMessage(Localizer["Message.DeleteError"], MessageType.Error);
        }
    }

    private void Edit(M.PhoneNumber item)
    {
        var url = this.ComposeUrl(
            basePath: PageState.Page.Path, 
            moduleId: ModuleState.ModuleId, 
            action: "Edit", 
            _providerId, item.PhoneNumberId );

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


    static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
        return (int)statusCode >= 200 && (int)statusCode <= 299; 
    }
}

