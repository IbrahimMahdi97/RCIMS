namespace Shared.DataTransferObjects;

public class AccountDto : AccountForManipulationDto
{
    public string UUID { get; set; }
    public int UnmatchedBalance { get; set; }
}