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

namespace OpenEugene.Module.LittleHelpBook;

public partial class Index : ModuleBase
{
    List<Models.LittleHelpBook> _LittleHelpBooks;
		
    [Inject] public LittleHelpBookService LittleHelpBookService { get; set; }
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
            (_LittleHelpBooks, var code) = await LittleHelpBookService.GetLittleHelpBooksAsync();
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

    private async Task Delete(Models.LittleHelpBook LittleHelpBook)
    {
        try
        {
            var code = await LittleHelpBookService.DeleteLittleHelpBookAsync(LittleHelpBook.LittleHelpBookId);
            if (!IsSuccessStatusCode(code)) {
                throw new HttpRequestException($"Error Deleting LittleHelpBooks. id:{LittleHelpBook.LittleHelpBookId}, Code: {code}");
            }
            await logger.LogInformation("LittleHelpBook Deleted {LittleHelpBook}", LittleHelpBook);

            (_LittleHelpBooks, code ) = await LittleHelpBookService.GetLittleHelpBooksAsync();

            StateHasChanged();
        }
        catch (Exception ex)
        {
            await logger.LogError(ex, "Error Deleting LittleHelpBook {LittleHelpBook} {Error}", LittleHelpBook, ex.Message);
            AddModuleMessage(Localizer["Message.DeleteError"], MessageType.Error);
        }
    }

     static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
        return (int)statusCode >= 200 && (int)statusCode <= 299; 
    }
}

