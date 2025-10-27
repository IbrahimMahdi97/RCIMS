using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;

namespace Presentation.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IServiceManager _service;
    public AccountsController(IServiceManager service) => _service = service;
    
    [Authorize(Roles = "admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AccountForManipulationDto account)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.AccountService.Create(account, userId);
        return id > 0 ? Ok(new { Id = id }) : BadRequest();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
    {
        var result = await _service.AccountService.GetAll();
        return Ok(result);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getById")]
    public async Task<ActionResult<AccountDto>> GetById(string id)
    {
        var result = await _service.AccountService.GetById(id);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("getByNameType")]
    public async Task<ActionResult<AccountDto>> GetByNameType(string name, int type)
    {
        var result = await _service.AccountService.GetByNameType(name, type);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] AccountForManipulationDto account, string id)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        await _service.AccountService.Update(account, id, userId);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        await _service.AccountService.Delete(id);
        return NoContent();
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getAccountType")]
    [HttpGet("getAccountType/{id:int}")]
    public IActionResult GetCommitteeType(int? id)
    {
        if (id.HasValue)
        {
            if (!Enum.IsDefined(typeof(AccountTypeEnum), id.Value))
                return BadRequest("Invalid account type value");

            var value = (AccountTypeEnum)id.Value;
            return Ok(new
            {
                Value = (int)value,
                Name = value.ToString()
            });
        }
        var values = Enum.GetValues(typeof(AccountTypeEnum))
            .Cast<AccountTypeEnum>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString()
            });

        return Ok(values);
    }
}