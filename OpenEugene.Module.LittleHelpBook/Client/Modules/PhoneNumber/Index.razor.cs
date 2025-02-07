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

namespace OpenEugene.Module.PhoneNumber;

public partial class Index : ModuleBase
{
    List<LittleHelpBook.Models.PhoneNumber> _list;


    [Inject] public PhoneNumberService PhoneNumberService { get; set; }
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
    private int _providerId = -1;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var moduleSettings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);
            _settingsVM = new SettingsViewModel(SettingService, moduleSettings);
            _providerId = Int32.Parse(PageState.QueryString["id"]);
            (_list, var code) = await PhoneNumberService.GetPhoneNumbersAsync(_providerId);
            if (!IsSuccessStatusCode(code)) {
                throw new HttpRequestException($"Error loading Phone Numbers. Code: {code}");
            }

            IsLoaded = true;
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Loading LittleHelpBook {Error}", ex.Message);
            AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
        }
    }

    private async Task Delete(M.PhoneNumber phone)
    {
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

     static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
        return (int)statusCode >= 200 && (int)statusCode <= 299; 
    }
}

