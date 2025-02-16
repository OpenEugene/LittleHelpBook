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
using M = OpenEugene.Module.LittleHelpBook.Models;

using OpenEugene.Module.LittleHelpBook.Services;


namespace OpenEugene.Module.Address
{
    public partial class Edit: ModuleBase
    {
		[Inject] public AddressService AddressService { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IStringLocalizer<Edit> Localizer { get; set; }		
        [Inject] public ISettingService SettingService { get; set; }


        private MudForm mudform;
        private bool success = false;
        private SettingsViewModel _settingsVM;
        private bool IsLoaded = false;
        private M.Address _item { get; set; } = new();
        private int _id = -1;
        private int _LittleHelpBookId;

		public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;

		public override string Actions => "Add,Edit";

		public override string Title => "Manage Address";

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
		    }
		    catch (Exception ex)
		    {
			    await logger.LogError(ex, "Error Loading Setings {Error}",  ex.Message);
			    AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
		    }
	    }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (PageState.Action == "Edit")
                {
                    _id = Int32.Parse(PageState.QueryString["id"]);
                    (_item, var code) = await AddressService.GetAddressAsync(_id);
                    if (!IsSuccessStatusCode(code))
                    {
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
                        (_item, var code) = await AddressService.AddAddressAsync(_item);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error Adding {_item}. Code: {code}");
                        }    
                        await logger.LogInformation("LittleHelpBook Added {_item}", _item);
                    }
                    else
                    {
                        (var latest, var code) = await AddressService.GetAddressAsync(_id);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error loading LittleHelpBook. Code: {code}");
                        }
                    
                        // update values from the local version of LittleHelpBook
                        latest.Address1 = _item.Address1;
                        latest.Address2 = _item.Address2;
                        latest.City = _item.City;
                        latest.State = _item.State;
                        latest.Latitude = _item.Latitude;
                        latest.Longitude = _item.Longitude;
                        latest.IsActive = _item.IsActive;
              
                        // update Database with the latest version of LittleHelpBook
                        (_item, code) = await AddressService.AddAddressAsync(_item);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error Updating Address. Code: {code}");
                        }         
                        await logger.LogInformation("LittleHelpBook Updated {latest}", latest);
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
