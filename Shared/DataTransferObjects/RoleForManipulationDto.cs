namespace Shared.DataTransferObjects;

public class RoleForManipulationDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}