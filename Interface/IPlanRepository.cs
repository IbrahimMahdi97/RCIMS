using Shared.DataTransferObjects;

namespace Interface;

public interface IPlanRepository
{
    Task<int> Create(PlanForManipulationDto plan, int userId);
    Task<IEnumerable<PlanDto>> GetAll();
    Task<PlanDto> GetById(string id);
    Task Update(PlanForManipulationDto plan, string id, int userId);
    Task Delete(string id);
    Task<PlanDto> GetByName(string name);
    Task<int> GetIdByUUID(string uuid);
}