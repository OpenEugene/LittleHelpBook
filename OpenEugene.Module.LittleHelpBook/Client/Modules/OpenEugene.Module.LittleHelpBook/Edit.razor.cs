using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;
using Oqtane.Services;

using OpenEugene.Module.LittleHelpBook.Services;


namespace OpenEugene.Module.LittleHelpBook
{
    public partial class Edit: ModuleBase
    {
		[Inject] public LittleHelpBookService LittleHelpBookService { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IStringLocalizer<Edit> Localizer { get; set; }		
        [Inject] public ISettingService SettingService { get; set; }

        private MudForm mudform;
        private bool success = false;
        private SettingsViewModel _settingsVM;
        private bool IsLoaded = false;
        private Models.LittleHelpBook LittleHelpBook { get; set; } = new();
        private int _LittleHelpBookId;

		public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;

		public override string Actions => "Add,Edit";

		public override string Title => "Manage LittleHelpBook";

        public override List<Resource> Resources => new List<Resource>()
        {
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" },
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = "_content/MudBlazor/MudBlazor.min.css" },
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = ModulePath() + "Module.css" },
            new Resource { ResourceType = ResourceType.Script,     Url = "_content/MudBlazor/MudBlazor.min.js", Location = ResourceLocation.Body, Level = ResourceLevel.Site },
            new Resource { ResourceType = ResourceType.Script,      Url = ModulePath() + "Module.js" },
        };

        protected override async Task OnInitializedAsync()
	    {
		    try
		    {
                var moduleSettings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);
                _settingsVM = new SettingsViewModel(SettingService, moduleSettings);
			    if (PageState.Action == "Edit")
			    {
				    _LittleHelpBookId = Int32.Parse(PageState.QueryString["id"]);
                    (LittleHelpBook, var code) = await LittleHelpBookService.GetLittleHelpBookAsync(_LittleHelpBookId);
                    if (!IsSuccessStatusCode(code)) {
                        throw new HttpRequestException($"Error loading LittleHelpBook. Code: {code}");
                    }
			    }
                IsLoaded = true;
            }
		    catch (Exception ex)
		    {
			    await logger.LogError(ex, "Error Loading LittleHelpBook {LittleHelpBookId} {Error}", _LittleHelpBookId, ex.Message);
			    AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
		    }
	    }

		private async Task Save()
		{
            try
            {
				await mudform.Validate();
				
                if (mudform.IsValid)
                {
                    if (PageState.Action == "Add")
                    {
                        LittleHelpBook.ModuleId = ModuleState.ModuleId;
                        (LittleHelpBook, var code) = await LittleHelpBookService.AddLittleHelpBookAsync(LittleHelpBook);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error Adding {LittleHelpBook}. Code: {code}");
                        }    
                        await logger.LogInformation("LittleHelpBook Added {LittleHelpBook}", LittleHelpBook);
                    }
                    else
                    {
                        (var LittleHelpBookLatest, var code) = await LittleHelpBookService.GetLittleHelpBookAsync(_LittleHelpBookId);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error loading LittleHelpBook. Code: {code}");
                        }
                    
                        // update values from the local version of LittleHelpBook
                        LittleHelpBookLatest.Name = LittleHelpBook.Name;
                        // update Database with the latest version of LittleHelpBook
                        (LittleHelpBook, code) = await LittleHelpBookService.AddLittleHelpBookAsync(LittleHelpBookLatest);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error Adding {LittleHelpBook}. Code: {code}");
                        }         
                        await logger.LogInformation("LittleHelpBook Updated {LittleHelpBookLatest}", LittleHelpBookLatest);
                    }
                    NavigationManager.NavigateTo(NavigateUrl());
                }
                else
                {
                    AddModuleMessage(Localizer["Message.SaveValidation"], MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                await logger.LogError(ex, "Error Saving LittleHelpBook {Error}", ex.Message);
                AddModuleMessage(Localizer["Message.SaveError"], MessageType.Error);
            }
		}
    
        static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
            return (int)statusCode >= 200 && (int)statusCode <= 299; 
        }
    }
}
