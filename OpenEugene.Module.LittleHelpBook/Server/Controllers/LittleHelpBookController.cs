using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OpenEugene.Module.LittleHelpBook.Repository;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;
using System;

namespace OpenEugene.Module.LittleHelpBook.Controllers;

[Route(ControllerRoutes.ApiRoute)]
public class LittleHelpBookController : ModuleControllerBase
{
    private readonly LittleHelpBookRepository _LittleHelpBookRepository;

    public LittleHelpBookController(LittleHelpBookRepository LittleHelpBookRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
    {
        _LittleHelpBookRepository = LittleHelpBookRepository;
    }

    // GET: api/<controller>?moduleid=x
    [HttpGet]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<IEnumerable<Models.LittleHelpBook>>> Get(){
        try{
            var data = _LittleHelpBookRepository.GetLittleHelpBooks();
            return Ok(data);
        }
        catch(Exception ex){
            var errorMessage = $"Repository Error Get Attempt LittleHelpBook";
            _logger.Log(LogLevel.Error, this, LogFunction.Read, errorMessage);
            return StatusCode(500);
        }
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<Models.LittleHelpBook>> Get(int id)
    {
        try {
            var data = _LittleHelpBookRepository.GetLittleHelpBook(id);
            return Ok(data);
        }
        catch (Exception ex)       { 
            _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed LittleHelpBook Get Attempt {id}", id);
            return StatusCode(500);
        }
    }

    // POST api/<controller>
    [HttpPost]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<Models.LittleHelpBook>> Post([FromBody] Models.LittleHelpBook LittleHelpBook)
    {
        if (ModelState.IsValid)
        {
            try{
                LittleHelpBook = _LittleHelpBookRepository.AddLittleHelpBook(LittleHelpBook);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "LittleHelpBook Added {LittleHelpBook}", LittleHelpBook);
            }
            catch (Exception ex) {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Failed LittleHelpBook Add Attempt {LittleHelpBook} Message {Message} ", LittleHelpBook,ex.Message);
                return StatusCode(500);
            }
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Create, "Invaid LittleHelpBook Post Attempt {LittleHelpBook}", LittleHelpBook);
            return BadRequest();
        }
        return Ok(LittleHelpBook);
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult<Models.LittleHelpBook>> Put(int id, [FromBody] Models.LittleHelpBook LittleHelpBook)
    {
        if (ModelState.IsValid && _LittleHelpBookRepository.GetLittleHelpBook(LittleHelpBook.LittleHelpBookId, false) != null)
        {
            LittleHelpBook = _LittleHelpBookRepository.UpdateLittleHelpBook(LittleHelpBook);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "LittleHelpBook Updated {LittleHelpBook}", LittleHelpBook);
            return Ok(LittleHelpBook);
        }
        else
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Update, "Unauthorized LittleHelpBook Put Attempt {LittleHelpBook}", LittleHelpBook);
            return BadRequest();
        }
    }

    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    [Authorize(Roles = RoleNames.Registered)]
    public async Task<ActionResult> Delete(int id)
    {
        var data = _LittleHelpBookRepository.GetLittleHelpBook(id);
        if (data is null)
        {
            _logger.Log(LogLevel.Error, this, LogFunction.Delete, "Failed LittleHelpBook Delete Attempt {LittleHelpBookId}", id);
            return NotFound();
        }

        _LittleHelpBookRepository.DeleteLittleHelpBook(id);
        _logger.Log(LogLevel.Information, this, LogFunction.Delete, "LittleHelpBook Deleted {LittleHelpBookId}", id);
        return Ok();
    
    }
}