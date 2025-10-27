using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;
using Shared.Parameters;

namespace Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IServiceManager _service;
    public UsersController(IServiceManager service) => _service = service;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        var userDto = await _service.UserService.ValidateUser(user);
        return Ok(userDto);
    }

    [Authorize(Roles = "admin,user")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto userForCreationDto)
    {
        var creatorId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.UserService.CreateUser(userForCreationDto, creatorId);
        return id > 0 ? Ok(new {Id = id}) : BadRequest();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("getUserDetails")]
    public async Task<ActionResult<UserDto>> GetUserDetails(string id)
    {
        var user = await _service.UserService.GetById(id);
        return user is not null ? Ok(user) : BadRequest();
    }

    [Authorize(Roles = "admin,user")]
    [HttpPut("updatePassword")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
    {
        await _service.UserService.UpdatePassword(updateUserPasswordDto);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMyDetails()
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        var user = await _service.UserService.GetMyDetails(userId);
        return user is not null ? Ok(user) : BadRequest();
    }

    [Authorize(Roles = "admin,user")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UserForCreationDto userForCreationDto, string id)
    {
        var updatedBy = User.RetrieveUserIdFromPrincipal();
        await _service.UserService.UpdateUser(userForCreationDto, id, updatedBy);
        return NoContent();
    }
    [Authorize(Roles = "admin")]
    [HttpGet("all")]
    public async Task<ActionResult<PagedList<UserForAllDto>>> GetAll([FromQuery] UsersAllParameters parameters)
    {
        var users = await _service.UserService.GetAll(parameters);
        return Ok(new { users, users.MetaData });
    }
    [Authorize(Roles = "admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var updatedBy = User.RetrieveUserIdFromPrincipal();
        await _service.UserService.Delete(id, updatedBy);
        return NoContent();
    }
}