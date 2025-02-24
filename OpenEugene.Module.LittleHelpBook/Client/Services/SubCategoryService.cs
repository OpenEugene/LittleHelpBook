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
    public class SubCategoryService : ResponseServiceBase, IService
    {
        public SubCategoryService(IHttpClientFactory http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("SubCategory");

        //mew pattern
        public async Task<(List<Models.SubCategory>, HttpStatusCode)> GetSubCategoriesAsync()
        {
            var url = $"{Apiurl}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.SubCategory>>(url);
            return (data, response.StatusCode);
        }

        public async Task<(List<Models.SubCategory>, HttpStatusCode)> GetSubCategoriesByCategoryAsync(int catId)
        {
            var url = $"{Apiurl}/category/{catId}";
            (var data, var response) = await GetJsonWithResponseAsync<List<Models.SubCategory>>(url);
            return (data, response.StatusCode);
        }

    }
}
