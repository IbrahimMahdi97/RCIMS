namespace Shared.DataTransferObjects;

public class UserForAllDto
{
    public string UUID { get; set; }
    public string? FullName { get; set; }
    public int Gender { get; set; }
    public DateTime Birthdate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? UserName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? RoleName { get; set; }
    public string RoleUUID { get; set; }
    public string? RoleDescription { get; set; }
}