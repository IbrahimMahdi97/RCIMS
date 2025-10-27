namespace Shared.DataTransferObjects;

public class PaymentForManipulationDto
{
    public string DocumentNumber { get; set; }
    public DateTime DocumentDate { get; set; }
    public int PaymentMethod { get; set; }
    public int PaymentType { get; set; }
    public int ContractId { get; set; }
    public int InstallmentId { get; set; }
    public int Amount { get; set; }
    public int Currency { get; set; }
    public int DocumentStatus { get; set; }
    public int DocumentFor { get; set; }
    public int AccountId { get; set; }
    public string Description { get; set; }
    public string Comments { get; set; }
}