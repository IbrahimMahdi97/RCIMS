namespace Shared.DataTransferObjects;

public class InstallmentForManipulationDto
{
    public int Contract { get; set; }
    public DateTime DueDate { get; set; }
    public int Status { get; set; }
    public int Amount { get; set; }
    public decimal Discount { get; set; }
    public int DiscountAmount { get; set; }
    public decimal PenaltyAccrued { get; set; }
    public int Subtotal { get; set; }
    public int PaidAmount { get; set; }
    public string Description { get; set; }
}