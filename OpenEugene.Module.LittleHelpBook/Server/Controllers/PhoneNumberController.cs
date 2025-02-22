using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Controllers;
using System.Net;
using OpenEugene.Module.LittleHelpBook.Models;
using OpenEugene.Module.LittleHelpBook.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;

namespace OpenEugene.Module.LittleHelpBook.Controllers;

[Route(ControllerRoutes.ApiRoute)]
public class PhoneNumberController : ModuleControllerBase
{
    private readonly LittleHelpBookRepository _LittleHelpBookRepository;

    public PhoneNumberController(LittleHelpBookRepository LittleHelpBookRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor) { 
        _LittleHelpBookRepository = LittleHelpBookRepository; 
    }

    // GET api/<controller>/5
    [HttpGet("provider/{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<PhoneNumber>> GetByProvider(int id)
    {
        try
        {
            var data = _LittleHelpBookRepository.GetPhoneNumbersByProviderId(id);
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed LittleHelpBook Get Attempt {id}", id);
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<PhoneNumber>> Get(int id)
    {
        try
        {
            var data = _LittleHelpBookRepository.GetPhoneNumberByPhoneNumberId(id);
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed Phone Number Get Attempt {id}", id);
            return StatusCode(500);
        }
    }


    // POST api/<controller>
    [HttpPost]
    public PhoneNumber Post([FromBody] PhoneNumber item)
    {
        if (ModelState.IsValid)
        {
            item = _LittleHelpBookRepository.AddPhoneNumber(item);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "PhoneNumber Added {item}", item);
        }
        else
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            item = null;
        }

        return item;
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<Models.PhoneNumber>> Put(int id, [FromBody] Models.PhoneNumber phone)
    {
        if (ModelState.IsValid && _LittleHelpBookRepository.GetPhoneNumber(phone.PhoneNumberId,false) != null)
        {
            phone = _LittleHelpBookRepository.UpdatePhoneNumber(phone);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Phone Updated {phone}", phone);
            return Ok(phone);
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Update, "error updating phone {phone}", phone);
            return BadRequest();
        }
    }


    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var item = _LittleHelpBookRepository.GetPhoneNumberByPhoneNumberId(id);
        if (item != null)
        {
            _LittleHelpBookRepository.DeletePhoneNumber(id);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "PhoneNumber Deleted {id}", id);
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Bad PhoneNumber Delete Attempt {id}", id);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}