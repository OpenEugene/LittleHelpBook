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
    public class CategoryService : ResponseServiceBase, IService
    {
        public CategoryService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Category");

        //mew pattern
        public async Task<(List<Models.Category>, HttpStatusCode)> GetCategoriesAsync()
        {
            var url = $"{Apiurl}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.Category>>(url);

            return (data, response.StatusCode);
        }

    }
}
