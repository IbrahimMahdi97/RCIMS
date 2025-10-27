using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;

namespace Presentation.Controllers;

[Route("api/roles")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IServiceManager _service;
    public RolesController(IServiceManager service) => _service = service;

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetAll()
    {
        var roles = await _service.RoleService.GetAll();
        return Ok(roles);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] RoleForManipulationDto roleForCreationDto)
    {
        var creatorId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.RoleService.Create(roleForCreationDto, creatorId);
        return id > 0 ? Ok(new { Id = id }) : BadRequest();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromBody] RoleForManipulationDto roleDto, int id)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        await _service.RoleService.Update(roleDto, id, userId);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        await _service.RoleService.Delete(id,userId);
        return NoContent();
    }
}