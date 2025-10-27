using Entities.Exceptions;
using Interface;
using Service.Interface;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class PlanService : IPlanService
{
    private readonly IRepositoryManager _repository;

    public PlanService(IRepositoryManager repository)
    {
        _repository = repository;
    }
    
    public async Task<int> Create(PlanForManipulationDto plan, int userId)
    {
        var alreadyExistAccount = GetByName(plan.Name);
        if (alreadyExistAccount != null)
            throw new PlanAlreadyExistsBadRequestException(plan.Name);
        var result = await _repository.Plan.Create(plan, userId);
        return result;
    }

    public async Task<IEnumerable<PlanDto>> GetAll()
    {
        var result = await _repository.Plan.GetAll();
        return result;
    }

    public async Task<PlanDto> GetById(string id)
    {
        var result = await _repository.Plan.GetById(id) ?? throw new PlanNotFoundException();
        return result;
    }

    public async Task Update(PlanForManipulationDto plan, string id, int userId)
    {
        var oldPlan = await GetById(id);
        await _repository.Plan.Update(plan, id, userId);
    }

    public async Task Delete(string id)
    {
        var plan = await GetById(id);
        await _repository.Plan.Delete(id);
    }

    public async Task<PlanDto> GetByName(string name)
    {
        var plan = await _repository.Plan.GetByName(name);
        return plan;
    }
}