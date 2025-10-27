namespace Shared.DataTransferObjects;

public class AssetForManipulationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int AssetType { get; set; }
    public int Area { get; set; }
    public int Price { get; set; }
    public int ListPrice { get; set; }
    public int Status { get; set; }
}