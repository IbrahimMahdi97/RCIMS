namespace Shared.DataTransferObjects;

public class UserForManipulationDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Gender { get; set; }
    public DateTime Birthdate { get; set; }
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Description { get; set; }
    public string? Comment { get; set; }
}