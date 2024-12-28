using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using System.Net;

namespace OpenEugene.Module.LittleHelpBook.Services
{
    public class LittleHelpBookService : ResponseServiceBase, IService
    {
        public LittleHelpBookService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("LittleHelpBook");

        public async Task<(List<Models.LittleHelpBook>,HttpStatusCode)> GetLittleHelpBooksAsync()
        {
            var url = $"{Apiurl}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.LittleHelpBook>>(url);
            return (data, response.StatusCode);      
        }

        public async Task<(Models.LittleHelpBook,HttpStatusCode)> GetLittleHelpBookAsync(int LittleHelpBookId)
        {
            var url = $"{Apiurl}/{LittleHelpBookId}";
            (var data, var response) = await GetJsonWithResponseAsync<Models.LittleHelpBook>(url);
            return (data, response.StatusCode);        
        }

        public async Task<(Models.LittleHelpBook,HttpStatusCode)> AddLittleHelpBookAsync(Models.LittleHelpBook LittleHelpBook)
        {
            var url = $"{Apiurl}";
            (var data, var response) = await PostJsonWithResponseAsync<Models.LittleHelpBook>(url,LittleHelpBook);
            return (data, response.StatusCode);        
        }

        public async Task<(Models.LittleHelpBook,HttpStatusCode)> UpdateLittleHelpBookAsync(Models.LittleHelpBook LittleHelpBook)
        {
            var url = $"{Apiurl}/{LittleHelpBook.LittleHelpBookId}";
            (var data, var response) = await PutJsonWithResponseAsync<Models.LittleHelpBook>(url,LittleHelpBook);
            return (data, response.StatusCode);        
        }

        public async Task<HttpStatusCode> DeleteLittleHelpBookAsync(int LittleHelpBookId)
        {
            var url = $"{Apiurl}/{LittleHelpBookId}";
            var response  = await DeleteWithResponseAsync(url);
            return response.StatusCode;
        }
    }
}
