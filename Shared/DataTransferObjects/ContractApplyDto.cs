namespace Shared.DataTransferObjects;

public class ContractApplyDto
{
    public string AssetId { get; set; } = null!;
    public string PlanId { get; set; } = null!;
    public string? Description { get; set; }
}