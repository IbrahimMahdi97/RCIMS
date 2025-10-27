using Shared.DataTransferObjects;

namespace Interface;

public interface IRoleRepository
{
    Task<IEnumerable<UserRoleDto>> GetUserRoles(int id);
    Task<IEnumerable<UserRoleDto>> GetAll();
    Task<int> Create(RoleForManipulationDto roleForCreationDto, int creatorId);
    Task<UserRoleDto> GetById(int id);
    Task Update(RoleForManipulationDto roleDto, int id, int userId);
    Task Delete(int id,int userId);
}