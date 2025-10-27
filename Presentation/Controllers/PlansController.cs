using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;

namespace Presentation.Controllers;

[Route("api/plans")]
[ApiController]
public class PlansController : ControllerBase
{
    private readonly IServiceManager _service;
    public PlansController(IServiceManager service) => _service = service;
    
    [Authorize(Roles = "admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PlanForManipulationDto plan)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.PlanService.Create(plan, userId);
        return id > 0 ? Ok(new { Id = id }) : BadRequest();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlanDto>>> GetAll()
    {
        var result = await _service.PlanService.GetAll();
        return Ok(result);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getById")]
    public async Task<ActionResult<PlanDto>> GetById(string id)
    {
        var result = await _service.PlanService.GetById(id);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("getByName")]
    public async Task<ActionResult<PlanDto>> GetByName(string name)
    {
        var result = await _service.PlanService.GetByName(name);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] PlanForManipulationDto plan, string id)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        await _service.PlanService.Update(plan, id, userId);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        await _service.PlanService.Delete(id);
        return NoContent();
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getPlanCalculationType")]
    [HttpGet("getPlanCalculationType/{id:int}")]
    public IActionResult GetCommitteeType(int? id)
    {
        if (id.HasValue)
        {
            if (!Enum.IsDefined(typeof(PlanCalculationTypeEnum), id.Value))
                return BadRequest("Invalid plan calculation type value");

            var value = (PlanCalculationTypeEnum)id.Value;
            return Ok(new
            {
                Value = (int)value,
                Name = value.ToString()
            });
        }
        var values = Enum.GetValues(typeof(PlanCalculationTypeEnum))
            .Cast<PlanCalculationTypeEnum>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString()
            });

        return Ok(values);
    }
}