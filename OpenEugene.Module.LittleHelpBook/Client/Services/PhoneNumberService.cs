using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;


namespace OpenEugene.Module.LittleHelpBook.Services
{
    public class PhoneNumberService : ResponseServiceBase, IService
    {
        public PhoneNumberService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("PhoneNumber");

        public async Task<(List<Models.PhoneNumber>, HttpStatusCode)> GetPhoneNumbersAsync(int id)
        {
            var url = $"{Apiurl}/provider/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.PhoneNumber>>(url);
            return (data, response.StatusCode);
        }

        public async Task<Models.PhoneNumber> AddPhoneNumberAsync(Models.PhoneNumber item)
        {
            item.EnsureIAuditable();
            return await PostJsonAsync<Models.PhoneNumber>($"{Apiurl}", item);
            
        }

        public async Task DeletePhoneNumberAsync(int id)
        {
            await DeleteAsync($"{Apiurl}/{id}");
        }
    }
}
