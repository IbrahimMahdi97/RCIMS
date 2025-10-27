namespace Shared.DataTransferObjects;

public class UserForCreationDto : UserForManipulationDto
{
    public string Password { get; set; } = null!;
    public int Role { get; set; }
}