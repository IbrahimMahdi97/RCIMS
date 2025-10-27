using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;

namespace Presentation.Controllers;

[Route("api/contracts")]
[ApiController]
public class ContractsController : ControllerBase
{
    private readonly IServiceManager _service;
    public ContractsController(IServiceManager service) => _service = service;
    
    [Authorize(Roles = "user")]
    [HttpPost("applyForContract")]
    public async Task<IActionResult> ApplyForContract([FromBody] ContractApplyDto contract)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.ContractService.ApplyForContract(contract, userId);
        return id > 0 ? Ok(new { Id = id }) : BadRequest();
    }
    
    [Authorize(Roles = "user")]
    [HttpPost("createByAdmin")]
    public async Task<IActionResult> CreateByAdmin([FromBody] ContractCreateByAdminDto contract)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.ContractService.CreateByAdmin(contract, userId);
        return id > 0 ? Ok(new { Id = id }) : BadRequest();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractDto>>> GetAll()
    {
        var result = await _service.ContractService.GetAll();
        return Ok(result);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getById")]
    public async Task<ActionResult<ContractDto>> GetById(string id)
    {
        var result = await _service.ContractService.GetById(id);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] ContractForManipulationDto contract, string id)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        await _service.ContractService.Update(contract, id, userId);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        await _service.ContractService.Delete(id);
        return NoContent();
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getContractStatus")]
    [HttpGet("getContractStatus/{id:int}")]
    public IActionResult GetCommitteeType(int? id)
    {
        if (id.HasValue)
        {
            if (!Enum.IsDefined(typeof(ContractStatusEnum), id.Value))
                return BadRequest("Invalid asset type value");

            var value = (ContractStatusEnum)id.Value;
            return Ok(new
            {
                Value = (int)value,
                Name = value.ToString()
            });
        }
        var values = Enum.GetValues(typeof(ContractStatusEnum))
            .Cast<ContractStatusEnum>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString()
            });

        return Ok(values);
    }
}