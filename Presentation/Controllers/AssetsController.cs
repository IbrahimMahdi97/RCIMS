using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Shared.DataTransferObjects;
using Shared.Helpers;

namespace Presentation.Controllers;

[Route("api/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly IServiceManager _service;
    public AssetsController(IServiceManager service) => _service = service;
    
    [Authorize(Roles = "admin")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AssetForManipulationDto asset)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        var id = await _service.AssetService.Create(asset, userId);
        return id > 0 ? Ok(new { Id = id }) : BadRequest();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAll()
    {
        var result = await _service.AssetService.GetAll();
        return Ok(result);
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getById")]
    public async Task<ActionResult<AssetDto>> GetById(string id)
    {
        var result = await _service.AssetService.GetById(id);
        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] AssetForManipulationDto asset, string id)
    {
        var userId = User.RetrieveUserIdFromPrincipal();
        await _service.AssetService.Update(asset, id, userId);
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        await _service.AssetService.Delete(id);
        return NoContent();
    }
    
    [Authorize(Roles = "admin")]
    [HttpGet("getAssetType")]
    [HttpGet("getAssetType/{id:int}")]
    public IActionResult GetCommitteeType(int? id)
    {
        if (id.HasValue)
        {
            if (!Enum.IsDefined(typeof(AssetTypeEnum), id.Value))
                return BadRequest("Invalid asset type value");

            var value = (AssetTypeEnum)id.Value;
            return Ok(new
            {
                Value = (int)value,
                Name = value.ToString()
            });
        }
        var values = Enum.GetValues(typeof(AssetTypeEnum))
            .Cast<AssetTypeEnum>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString()
            });

        return Ok(values);
    }
}