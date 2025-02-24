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
    public class AttributeService : ResponseServiceBase, IService
    {
        public AttributeService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Attribute");

        //mew pattern
        public async Task<(List<Models.Attribute>, HttpStatusCode)> GetAttributesAsync()
        {
            var url = $"{Apiurl}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.Attribute>>(url);
            return (data, response.StatusCode);
        }

        // old pattern
        public async Task<Models.Attribute> AddAttributeAsync(Models.Attribute item)
        {
            item.EnsureIAuditable();
            return await PostJsonAsync<Models.Attribute>($"{Apiurl}", item);
        }

        public async Task DeleteAddressAsync(int id)
        {
            await DeleteAsync($"{Apiurl}/{id}");
        }
    }
}
