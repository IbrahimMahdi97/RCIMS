namespace Shared.DataTransferObjects;

public class UpdateUserPasswordDto
{
    public string Id { get; set; } = null!;
    public string Password { get; set; } = null!;
}