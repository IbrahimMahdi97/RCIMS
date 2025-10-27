using Shared.DataTransferObjects;

namespace Service.Interface;

public interface IRoleService
{
    Task<IEnumerable<UserRoleDto>> GetAll();
    Task<int> Create(RoleForManipulationDto roleForCreationDto, int creatorId);
    Task<UserRoleDto> GetById(int id);
    Task Update(RoleForManipulationDto roleDto, int id, int userId);
    Task Delete(int id,int userId);
}