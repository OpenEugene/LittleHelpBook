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
using OpenEugene.Module.LittleHelpBook.Client.Extensions;

using OpenEugene.Module.LittleHelpBook.Services;
using OpenEugene.Module.LittleHelpBook.Client.Viewmodels;


namespace OpenEugene.Module.ProviderHeader
{
    public partial class Edit: ModuleBase
    {
        [Inject] public ProviderService ProviderService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IStringLocalizer<Edit> Localizer { get; set; }		
        [Inject] public ISettingService SettingService { get; set; }


        private MudForm mudform;
        private bool success = false;
        private SettingsViewModel _settingsVM;
        private bool IsLoaded = false;
        private M.Provider _item = new();
        private int _id = -1;

		public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;

		public override string Actions => "Add,Edit";
        public override string UrlParametersTemplate => Routing.ProviderTemplate;
        public override string Title => "Provider";

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
                    _id = Int32.Parse(UrlParameters[Routing.ProviderId]);
                    (_item, var code) = await ProviderService.GetProviderAsync(_id);
                    if (!this.IsSuccessStatusCode(code))
                    {
                        throw new HttpRequestException($"Error loading Address. Code: {code}");
                    }
                }
                else {
                    _item = new M.Provider();
                }
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                await logger.LogError(ex, "Error Loading Address {Error}", ex.Message);
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
                        (_item, var code) = await ProviderService.AddProviderAsync(_item);
                        if (code is not HttpStatusCode.OK)
                        {
                            throw new HttpRequestException($"Error Adding {_item}. Code: {code}");
                        }
                        await logger.LogInformation("LittleHelpBook Added {_item}", _item);
                    }
                    else
                    {
                        (_item, var code) = await ProviderService.UpdateProviderAsync(_item);
                        if (code is not HttpStatusCode.OK)
                        {
                            throw new HttpRequestException($"Error Updating {_item}. Code: {_item}");
                        }
                        await logger.LogInformation("LittleHelpBook Updated {_item}", _item);
                    }

                    NavigationManager.NavigateTo(PageState.ReturnUrl);

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

    }
}
