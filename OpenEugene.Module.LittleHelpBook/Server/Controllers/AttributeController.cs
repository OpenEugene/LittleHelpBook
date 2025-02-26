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
    public class AttributeController : ModuleControllerBase
    {
        private readonly LittleHelpBookRepository _LittleHelpBookRepository;

        public AttributeController(LittleHelpBookRepository LittleHelpBookRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _LittleHelpBookRepository = LittleHelpBookRepository;
        }


        // GET: api/<controller>?moduleid=x
        [HttpGet]
        public IEnumerable<Models.Attribute> Get()
        {
            try
            {
                var list = _LittleHelpBookRepository.GetAttributes();
                return list;
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, ex, "Get Attributes Failed");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

        }

    [HttpGet("provider/{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<ProviderAttributeViewModel>> GetByProvider(int id)
    {
        try
        {
            var data = _LittleHelpBookRepository.GetProviderAttributesByProviderId(id);
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed Address Get Attempt {id}", id);
            return StatusCode(500);
        }
    }

    // POST api/<controller>
    [HttpPost("providerattributes")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<List<Models.ProviderAttribute>>> Post([FromBody] List<Models.ProviderAttribute> list)
    {
        var result = new List<Models.ProviderAttribute>();

        if (ModelState.IsValid)
        {
            try
            {
                // get the existing attributes for this provider
                var attribs = _LittleHelpBookRepository.GetProviderAttributesByProviderId(list.First().ProviderId);

                foreach (var item in list)
                {
                    // don't add dupes
                    if (attribs.Any(a=>a.ProviderAttributeId==item.ProviderAttributeId))
                    {
                        continue;
                    }
                    var newItem = _LittleHelpBookRepository.AddProviderAttribute(item);
                    result.Add(newItem);
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "Provider Attribute Added {item}", item);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Failed attribute Add Attempt {Message} ", ex.Message);
                return StatusCode(500);
            }
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Create, "Bad model state for add attributes {list}",list);
            return BadRequest();
        }
        return Ok(result);
    }

}

