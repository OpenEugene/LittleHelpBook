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
using System.Collections;
using OpenEugene.Module.LittleHelpBook.ViewModels;
using static MudBlazor.CategoryTypes;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;

namespace OpenEugene.Module.ProviderAttribute
{
    public partial class Add: ModuleBase
    {
		[Inject] public AttributeService AttributeService { get; set; }
        [Inject] public CategoryService CategoryService { get; set; }
        [Inject] public SubCategoryService SubCategoryService { get; set; }
        [Inject] public ProviderAttributeService ProviderAttributeService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IStringLocalizer<Add> Localizer { get; set; }		
        [Inject] public ISettingService SettingService { get; set; }

        private MudForm mudform;
        private bool success = false;
        private SettingsViewModel _settingsVM;
        private bool IsLoaded = false; 
        private int _providerid;
        private M.ProviderAttribute _item = new();
        private List<M.Category> _cats;
        private List<M.SubCategory> _subCats;
        private int? _selectedCatId;
        private IReadOnlyCollection<int> _selectedSubCatIds;


        public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;

		public override string Actions => "Add";

		public override string Title => "Service";

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

                (_cats, var code) = await CategoryService.GetCategoriesAsync();
                if (!IsSuccessStatusCode(code))
                {
                    throw new HttpRequestException($"Error loading attribute list. Code: {code}");
                }

            }
            catch (Exception ex)
            {
                await logger.LogError(ex, "Error Loading Provider Attribute settings {Error}", ex.Message);
                AddModuleMessage(Localizer["Message.LoadError"], MessageType.Error);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!ShouldRender()) return;

            if (UrlParameters.ContainsKey(Routing.ProviderId))
            {
                _providerid = Int32.Parse(UrlParameters[Routing.ProviderId]);

                _item = new()
                {
                    ProviderId = _providerid
                };
        
                IsLoaded = true;
            }
        }

        protected async Task OnCategoryChange(int? catId)
        {
            try
            {
                _selectedCatId = catId;
                (_subCats, var code) = await SubCategoryService.GetSubCategoriesByCategoryAsync(_selectedCatId.Value);
                if (!IsSuccessStatusCode(code))
                {
                    throw new HttpRequestException($"Error loading subcategories. Code: {code}");
                }
            }
            catch (Exception ex)
            {
                await logger.LogError(ex, "Error Loading SubCategories {Error}", ex.Message);
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
                    List<M.ProviderAttribute> _items = new();

                    foreach (var subCatId in _selectedSubCatIds)
                    {
                        _items.Add(new M.ProviderAttribute
                        {
                            ProviderId = _providerid,
                            ProviderAttributeId = _item.ProviderAttributeId,
                        });
                    }     
                  
                    (_items, var code) = await ProviderAttributeService.AddAttributesAsync(_items);
                    if (code is not HttpStatusCode.OK) {
                        throw new HttpRequestException($"Error Adding Attributes. Code: {code}");
                    }    
                    await logger.LogInformation("Attributes Added", _items);

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
    
        static bool IsSuccessStatusCode(HttpStatusCode statusCode) { 
            return (int)statusCode >= 200 && (int)statusCode <= 299; 
        }
    }
}
