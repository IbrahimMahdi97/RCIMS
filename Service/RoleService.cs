using Entities.Exceptions;
using Interface;
using Service.Interface;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class RoleService : IRoleService
{
    private readonly IRepositoryManager _repository;

    public RoleService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserRoleDto>> GetAll()
    {
        var roles = await _repository.Role.GetAll();
        return roles;
    }
    public async Task<int> Create(RoleForManipulationDto roleForCreationDto, int creatorId)
    { 
        var result = await _repository.Role.Create(roleForCreationDto, creatorId);
        return result;
    }
    public async Task<UserRoleDto> GetById(int id)
    {
        var role = await _repository.Role.GetById(id);
        return role is null ? throw new RoleNotFoundException(id) : role;
    }
    public async Task Update(RoleForManipulationDto roleDto, int id, int userId)
    {
        await GetById(id);
        await _repository.Role.Update(roleDto, id, userId);
    }
    public async Task Delete(int id,int userId)
    {
        await GetById(id);
        await _repository.Role.Delete(id,userId);
    }
}