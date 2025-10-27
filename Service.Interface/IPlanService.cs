using Shared.DataTransferObjects;

namespace Service.Interface;

public interface IPlanService
{
    Task<int> Create(PlanForManipulationDto plan, int userId);
    Task<IEnumerable<PlanDto>> GetAll();
    Task<PlanDto> GetById(string id);
    Task Update(PlanForManipulationDto plan, string id, int userId);
    Task Delete(string id);
    Task<PlanDto> GetByName(string name);
}