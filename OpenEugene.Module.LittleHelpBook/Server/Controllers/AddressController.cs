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

namespace OpenEugene.Module.LittleHelpBook.Controllers;

[Route(ControllerRoutes.ApiRoute)]
public class AddressController : ModuleControllerBase
{
    private readonly LittleHelpBookRepository _LittleHelpBookRepository;

    public AddressController(LittleHelpBookRepository LittleHelpBookRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
    {
        _LittleHelpBookRepository = LittleHelpBookRepository;
    }

    [HttpGet("provider/{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<Address>> GetByProvider(int id)
    {
        try
        {
            var data = _LittleHelpBookRepository.GetAddressesByProviderId(id);
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed Address Get Attempt {id}", id);
            return StatusCode(500);
        }
    }

    // POST api/<controller>
    [HttpPost]
    public async Task<ActionResult<Models.Address>> Post([FromBody] Models.Address item)
    {
        if (ModelState.IsValid )
        {
            item = _LittleHelpBookRepository.AddAddress(item);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Address Added {item}", item);
            return Ok(item);
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Update, "error updating Address {item}", item);
            return BadRequest();
        }
      
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<Models.Address>> Put(int id, [FromBody] Models.Address item)
    {
        if (ModelState.IsValid && _LittleHelpBookRepository.GetPhoneNumber(item.AddressId, false) != null)
        {
            item = _LittleHelpBookRepository.UpdateAddress(item);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Address Updated {item}", item);
            return Ok(item);
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Update, "error updating Address {item}", item);
            return BadRequest();
        }
    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var item = _LittleHelpBookRepository.GetAddressByAddressId(id);
        if (item != null )
        {
            _LittleHelpBookRepository.DeleteAddress(id);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Address Deleted {id}", id);
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Bad Address Delete Attempt {id}", id);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}

