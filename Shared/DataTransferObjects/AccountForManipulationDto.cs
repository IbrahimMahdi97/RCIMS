namespace Shared.DataTransferObjects;

public class AccountForManipulationDto
{
    public string Name { get; set; }
    public int AccountType { get; set; }
    public string OpenBalance { get; set; }
    public string Description { get; set; }
    public string Comments { get; set; }
}