namespace Shared.DataTransferObjects;

public class PlanForManipulationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CalculationType { get; set; }
    public int Month { get; set; }
    public int Equation { get; set; }
    public int Period { get; set; }
    public decimal Percent { get; set; }
    public int MaxAllow { get; set; }
}