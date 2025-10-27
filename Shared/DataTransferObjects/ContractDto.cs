namespace Shared.DataTransferObjects;

public class ContractDto : ContractForManipulationDto
{
    public string UUID { get; set; }
    public string ContractStatusName { get; set; }
    public string CustomerName { get; set; }
    public string AssetName { get; set; }
    public string AssetType { get; set; }
    public string Area { get; set; }
    public string PlanName { get; set; }
}