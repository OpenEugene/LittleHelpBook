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
using M = OpenEugene.Module.LittleHelpBook.Models;
using OpenEugene.Module.LittleHelpBook.Client.Viewmodels;
using Oqtane.UI;

namespace OpenEugene.Module.PhoneNumber
{
    public partial class Edit: ModuleBase
    {
		[Inject] public PhoneNumberService PhoneNumberService { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IStringLocalizer<Edit> Localizer { get; set; }		
        [Inject] public ISettingService SettingService { get; set; }

        private MudForm mudform;
        private bool success = false;
        private SettingsViewModel _settingsVM;
        private bool IsLoaded = false;
        private M.PhoneNumber _phone = new();
        private int _phoneId = -1;

        public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;

		public override string Actions => "Add,Edit";

		public override string Title => "Phone Number";

        public override List<Resource> Resources => new List<Resource>()
        {
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" },
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = "_content/MudBlazor/MudBlazor.min.css" },
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = ModulePath() + "Module.css" },
            new Resource { ResourceType = ResourceType.Script,     Url = "_content/MudBlazor/MudBlazor.min.js", Location = ResourceLocation.Body, Level = ResourceLevel.Site },
            new Resource { ResourceType = ResourceType.Script,      Url = ModulePath() + "Module.js" },
        };

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
			    await logger.LogError(ex, "Error Loading PhoneNumber settings {Error}", ex.Message);
			    AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
		    }
	    }

        protected override async Task OnParametersSetAsync()
        {
            if (!ShouldRender()) return;

            try
            {
                _phoneId = Int32.Parse(UrlParameters[Routing.PhoneId]);
                (_phone, var code) = await PhoneNumberService.GetPhoneNumberAsync(_phoneId);
                if (!IsSuccessStatusCode(code))
                {
                    throw new HttpRequestException($"Error loading PhoneNumber. Code: {code}");
                }
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                await logger.LogError(ex, "Error Loading PhoneNumber {Error}", ex.Message);
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
                        (_phone, var code) = await PhoneNumberService.AddPhoneNumberAsync(_phone);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error Adding {_phone}. Code: {code}");
                        }    
                        await logger.LogInformation("Phone Added {LittleHelpBook}", _phone);
                    }
                    else
                    {
                        (_phone, var code) = await PhoneNumberService.UpdatePhoneNumberAsync(_phone);

                        (var phoneLatest, var codeLatest) = await PhoneNumberService.GetPhoneNumberAsync(_phoneId);
                        if (codeLatest is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error loading phone. Code: {codeLatest}");
                        }
                    
                        // update values from the local version of LittleHelpBook
                        phoneLatest.Number = _phone.Number;
                        phoneLatest.AreaCode = _phone.AreaCode;
                        phoneLatest.Extension = _phone.Extension;
                        phoneLatest.Description = _phone.Description;

                        // update Database with the latest version of LittleHelpBook
                        (_phone, code) = await PhoneNumberService.UpdatePhoneNumberAsync(phoneLatest);
                        if (code is not HttpStatusCode.OK) {
                            throw new HttpRequestException($"Error Updating {_phone}. Code: {code}");
                        }         
                        await logger.LogInformation("LittleHelpBook Updated {phoneLatest}", phoneLatest);
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
