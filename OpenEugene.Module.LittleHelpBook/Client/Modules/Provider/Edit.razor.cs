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
using OpenEugene.Module.LittleHelpBook.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;


namespace OpenEugene.Module.Provider
{
    public partial class Edit: ModuleBase
    {
		[Inject] public ProviderService ProviderService { get; set; }
        [Inject] public PhoneNumberService PhoneNumberService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IStringLocalizer<Edit> Localizer { get; set; }		
        [Inject] public ISettingService SettingService { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private MudForm mudform;
        private SettingsViewModel _settingsVM;
        private bool isLoaded = false;
        private ProviderViewModel _model;
        private List<LittleHelpBook.Models.PhoneNumber> _orderedPhoneNumbers;
        private List<LittleHelpBook.Models.Address> _orderedAddresses;
        ValidationSummary summary;
        MudMessageBox Mbox { get; set; }
        public override bool UseAdminContainer => false;

        private int _id;

		public override string Actions => "Edit";

		public override string Title => "Manage Provider";

        public override List<Resource> Resources => new List<Resource>()
        {
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" },
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = "_content/MudBlazor/MudBlazor.min.css" },
            new Resource { ResourceType = ResourceType.Stylesheet,  Url = ModulePath() + "Module.css" },
            new Resource { ResourceType = ResourceType.Script,      Url = "_content/MudBlazor/MudBlazor.min.js", Location = ResourceLocation.Body, Level = ResourceLevel.Site },
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
                    _id = int.Parse(PageState.QueryString["id"]);
                    await Refresh();
                }
                isLoaded = true;
            }
		    catch (Exception ex)
		    {
			    await logger.LogError(ex, "Error Loading LittleHelpBook {LittleHelpBookId} {Error}", _id, ex.Message);
			    AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
		    }
	    }

        private async Task Refresh()
        {
           
            (_model, var code) = await ProviderService.GetProviderViewModelAsync(_id);
            if (!IsSuccessStatusCode(code))
            {
                throw new HttpRequestException($"Error loading Providers. Code: {code}");
            }
            _orderedPhoneNumbers = _model.PhoneNumbers.OrderByDescending(p => p.IsActive).ToList();
            _orderedAddresses = _model.Addresses.OrderByDescending(a => a.IsActive).ToList();

        }

        private async Task Save()
		{
            try
            {
				await mudform.Validate();
				
                if (mudform.IsValid)
                {

                    try
                    {
                       
                        (_model, var code) = await ProviderService.UpdateProviderAsync(_model);
                        if (code is not HttpStatusCode.OK)
                        {
                            throw new HttpRequestException($"Error updating Provider. Code: {code}");
                        }
                        await logger.LogInformation("Provider Updated {_model}", _model);
                        AddModuleMessage(Localizer["Message.UpdateSuccess"], MessageType.Success);
                    }
                    catch (Exception ex)
                    {
                        await logger.LogError(ex, "Error Saving LHB {id} {message}", _id, ex.Message);
                        AddModuleMessage(Localizer["Message.UpdateError"], MessageType.Error);
                    }

                    NavigationManager.NavigateTo(NavigateUrl(), true);
                   
                }
                else
                {
                    AddModuleMessage(Localizer["Message.SaveValidation"], MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                await logger.LogError(ex, "Error Saving Provider {Error}", ex.Message);
                AddModuleMessage(Localizer["Message.SaveError"], MessageType.Error);
            }
		}
    
        static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
            return (int)statusCode >= 200 && (int)statusCode <= 299; 
        }

        private void Back() { NavigationManager.NavigateTo(NavigateUrl()); }


        private async Task AddAttributes()
        {
            var parameters = new DialogParameters<AddAttribute>();
            parameters.Add(x => x.Id, _id);
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<AddAttribute>("Add Service", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Refresh();
                StateHasChanged();
            }
        }

        private async Task DeleteAttribute(ProviderAttributeViewModel item)
        {
            await ProviderService.DeleteAttributeAsync(item.ProviderAttributeId);
            await Refresh();
            StateHasChanged();
        }


        private async Task AddAddr()
        {
            var parameters = new DialogParameters<AddAddress>();
            parameters.Add(x => x.Id, _id);
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<AddAddress>("Add Address", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Refresh();
                StateHasChanged();
            }
        }

        private async Task DeleteAddr(LittleHelpBook.Models.Address addr)
        {
            // ask if they are sure
            var result = await Mbox.ShowAsync();
            if (result == true)
            {
                await Refresh();
                StateHasChanged();
            }
        }

        private async Task AddPhone()
        {
            var parameters = new DialogParameters<AddPhone>();
            parameters.Add(x => x.Id, _id);
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<AddPhone>("Add Phone Number", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Refresh();
                StateHasChanged();
            }
        }

        private async Task DeletePhoneNumber(LittleHelpBook.Models.PhoneNumber phone)
        {
            var result = await Mbox.ShowAsync();
            if (result == true)
            {
                await PhoneNumberService.DeletePhoneNumberAsync(phone.PhoneNumberId);
                await Refresh();
                StateHasChanged();
            }
        }
    }
}
