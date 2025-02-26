using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OpenEugene.Module.LittleHelpBook.ViewModels;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;


namespace OpenEugene.Module.LittleHelpBook.Services
{
    public class ProviderAttributeService : ResponseServiceBase, IService
    {
        public ProviderAttributeService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Attribute");

  
        public async Task<(List<ProviderAttributeViewModel>, HttpStatusCode)> GetAttributesByProviderAsync(int id)
        {
            var url = $"{Apiurl}/provider/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<List<ProviderAttributeViewModel>>(url);
            return (data, response.StatusCode);
        }


        public async Task<(Models.Attribute, HttpStatusCode)> AddAttributeAsync(Models.Attribute item)
        {
            item.EnsureIAuditable();
            var url = $"{Apiurl}";
            (var data, var response) = await PostJsonWithResponseAsync<Models.Attribute>(url,item);
            return (data, response.StatusCode);
        }

        public async Task<(List<Models.ProviderAttribute>, HttpStatusCode)> AddAttributesAsync(List<Models.ProviderAttribute> items)
        {
            foreach (var item in items)
            {
                item.EnsureIAuditable();
            }
            var url = $"{Apiurl}/providerattributes"; // plural
            (var data, var response) = await PostJsonWithResponseAsync<List<Models.ProviderAttribute>>(url, items);
            return (data, response.StatusCode);
        }


        public async Task DeleteAttributeAsync(int id)
        {
            await DeleteAsync($"{Apiurl}/{id}");
        }

        public async Task DeleteProviderAttributeAsync(int id)
        {
            await DeleteAsync($"{Apiurl}/providerattribute/{id}");
        }
    }
}
