using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;


namespace OpenEugene.Module.LittleHelpBook.Services
{
    public class AddressService : ResponseServiceBase, IService
    {
        public AddressService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Address");

   
        public async Task<(List<Models.Address>, HttpStatusCode)> GetAddressesAsync(int id)
        {
            var url = $"{Apiurl}/provider/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.Address>>(url);
            return (data, response.StatusCode);
        }

        public async Task<(Models.Address, HttpStatusCode)> GetAddressAsync(int id)
        {
            var url = $"{Apiurl}/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<Models.Address>(url);
            return (data, response.StatusCode);
        }

        public async Task<(Models.Address, HttpStatusCode)> AddAddressAsync(Models.Address item)
        {
            var url = $"{Apiurl}";
            (var data, var response) = await PostJsonWithResponseAsync(url, item);
            return (data, response.StatusCode);
        }


        public async Task<HttpStatusCode> DeleteAddressAsync(int id)
        {
            await DeleteAsync($"{Apiurl}/{id}");
            return HttpStatusCode.OK;
        }
    }
}
