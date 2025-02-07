using System;
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

        public async Task<(Models.PhoneNumber, HttpStatusCode)> GetPhoneNumberAsync(int id)
        {
            var url = $"{Apiurl}/{id}";
            (var data, var response) = await GetJsonWithResponseAsync<Models.PhoneNumber>(url);
            return (data, response.StatusCode);
        }


        public async Task<(Models.PhoneNumber,HttpStatusCode)> AddPhoneNumberAsync(Models.PhoneNumber item)
        {
            item.EnsureIAuditable();
            (var data, var response) = await PostJsonWithResponseAsync<Models.PhoneNumber>($"{Apiurl}", item);
            return (data, response.StatusCode);
        }

        public async Task<(Models.PhoneNumber, HttpStatusCode)> UpdatePhoneNumberAsync(Models.PhoneNumber item)
        {
            item.EnsureIAuditable();
            (var data, var response) = await PutJsonWithResponseAsync<Models.PhoneNumber>($"{Apiurl}", item);
            return (data, response.StatusCode);
        }

        public async Task<HttpStatusCode> DeletePhoneNumberAsync(int id)
        {
            var url = $"{Apiurl}/{id}";
            var response = await DeleteWithResponseAsync(url);
            return response.StatusCode;
        }

    }
}
