namespace Shared.DataTransferObjects;

public class ContractForManipulationDto
{
    public string ContractNumber { get; set; }
    public DateTime ContractDate { get; set; }
    public int ContractStatus { get; set; }
    public string? Description { get; set; }
    public int ContractFor { get; set; }
    public int Asset { get; set; }
    public int Plan { get; set; }
    public int TotalAmount { get; set; }
    public int DownPayment { get; set; }
    public int PaidAmount { get; set; }
    public decimal Discount { get; set; }
    public int DiscountAmount { get; set; }
    public int GraceDays { get; set; }
    public decimal DailyPenaltyRate { get; set; }
    public decimal EarlyPayoffDiscountPercent { get; set; }
    public int Subtotal { get; set; }
}