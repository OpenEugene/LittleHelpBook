using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using System.Net;
using OpenEugene.Module.LittleHelpBook.ViewModels;

namespace OpenEugene.Module.LittleHelpBook.Services
{
    public class ProviderService : ResponseServiceBase, IService
    {
        public ProviderService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Provider");

        public async Task<(List<Models.Provider>,HttpStatusCode)> GetProvidersAsync()
        {
            var url = $"{Apiurl}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.Provider>>(url);
            return (data, response.StatusCode);      
        }
        public async Task<(List<Models.Provider>, HttpStatusCode)> GetProvidersFilteredAsync(string[] filters)
        {
            var filterString = string.Join(",", filters);
            
            // encode the filter string to ensure it is safe to pass in the URL
            filterString = System.Web.HttpUtility.UrlEncode(filterString);

            var url = $"{Apiurl}/filtered/{filterString}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.Provider>>(url);
            return (data, response.StatusCode);
        }

        public async Task<(ProviderViewModel, HttpStatusCode)> GetProviderViewModelAsync(int id)
        {
            var url = $"{Apiurl}/vm/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<ProviderViewModel>(url);
            return (data, response.StatusCode);
        }

        public async Task<(Models.Provider, HttpStatusCode)> GetProviderAsync(int id)
        {
            var url = $"{Apiurl}/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<Models.Provider>(url);
            return (data, response.StatusCode);        
        }

        public async Task<(Models.Provider, HttpStatusCode)> AddProviderAsync(Models.Provider item)
        {
            item.EnsureIAuditable();
            var url = $"{Apiurl}";
            (var data, var response) = await PostJsonWithResponseAsync(url,item);
            return (data, response.StatusCode);        
        }

        public async Task<(Models.Provider, HttpStatusCode)> UpdateProviderAsync(Models.Provider item)
        {
            var url = $"{Apiurl}/{item.ProviderId}";
            (var data, var response) = await PutJsonWithResponseAsync<Models.Provider>(url,item);
            return (data, response.StatusCode);        
        }

        public async Task<(ProviderViewModel, HttpStatusCode)> UpdateProviderAsync(ProviderViewModel item)
        {
            var url = $"{Apiurl}/vm/{item.ProviderId}";
            (var data, var response) = await PutJsonWithResponseAsync<ProviderViewModel>(url, item);
            return (data, response.StatusCode);
        }

        public async Task<HttpStatusCode> DeleteProviderAsync(int id)
        {
            var url = $"{Apiurl}/{id}";
            var response  = await DeleteWithResponseAsync(url);
            return response.StatusCode;
        }

        public async Task DeleteAttributeAsync(int id)
        {
            await DeleteAsync($"{Apiurl}/ProviderAttribute/{id}");
        }
    }
}
