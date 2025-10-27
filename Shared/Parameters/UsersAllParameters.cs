namespace Shared.Parameters;

public class UsersAllParameters : RequestParameters
{
    public string? FullName { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Phone { get; set; } = null;
    public string? UserName { get; set; } = null;
    public int RoleId { get; set; } = 0;
}