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
using System.Linq;

namespace OpenEugene.Module.LittleHelpBook.Controllers;

[Route(ControllerRoutes.ApiRoute)]
public class CategoryController : ModuleControllerBase
{
    private readonly LittleHelpBookRepository _LittleHelpBookRepository;

    public CategoryController(LittleHelpBookRepository LittleHelpBookRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
    {
        _LittleHelpBookRepository = LittleHelpBookRepository;
    }

    // GET: api/<controller>?moduleid=x
    [HttpGet]
    public IEnumerable<Models.Category> Get()
    {
        try
        {
            var list = _LittleHelpBookRepository.GetCategories();
            return list.ToList();
        }
        catch (System.Exception ex)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Security, ex, "Get Categories Failed");
            HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return null;
        }

    }

}
