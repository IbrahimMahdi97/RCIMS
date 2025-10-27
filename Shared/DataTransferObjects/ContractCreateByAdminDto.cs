namespace Shared.DataTransferObjects;

public class ContractCreateByAdminDto : ContractApplyDto
{
    public string UserId { get; set; } = null!;
    public int DownPayment { get; set; }
    public decimal Discount { get; set; }
}