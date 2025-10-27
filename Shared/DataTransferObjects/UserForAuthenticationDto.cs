namespace Shared.DataTransferObjects;

public class UserForAuthenticationDto
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}