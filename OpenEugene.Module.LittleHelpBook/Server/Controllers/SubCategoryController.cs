using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Controllers;
using System.Net;

using Oqtane.Models;
using OpenEugene.Module.LittleHelpBook.Models;
using OpenEugene.Module.LittleHelpBook.Repository;
using System.Threading.Tasks;
using System;
using OpenEugene.Module.LittleHelpBook.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Linq;

namespace OpenEugene.Module.LittleHelpBook.Controllers;

    [Route(ControllerRoutes.ApiRoute)]
    public class SubCategoryController : ModuleControllerBase
    {
        private readonly LittleHelpBookRepository _LittleHelpBookRepository;

        public SubCategoryController(LittleHelpBookRepository LittleHelpBookRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _LittleHelpBookRepository = LittleHelpBookRepository;
        }


        // GET: api/<controller>?moduleid=x
        [HttpGet]
        public IEnumerable<Models.SubCategory> Get()
        {
            try
            {
                var list = _LittleHelpBookRepository.GetSubCategories();
                return list.OrderBy(i => i.Name);
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, ex, "Get SubCategories Failed");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

        }

    [HttpGet("category/{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<List<SubCategory>>> GetByCategory(int id)
    {
        try
        {
            var data = _LittleHelpBookRepository.GetSubCategoriesByCategory(id);
            return Ok(data.OrderBy(i=>i.Name));
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed Address Get Attempt {id}", id);
            return StatusCode(500);
        }
    }

}

