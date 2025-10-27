using Shared.DataTransferObjects;
using Shared.Parameters;

namespace Interface;

public interface IUserRepository
{
    Task<UserDto?> FindByCredentialsUsername(string username, string password);
    Task<IEnumerable<UserRoleDto>> GetUserRoles(int userId);
    Task<int> FindIdByUsername(string username);
    Task UpdateRefreshToken(int id, string refreshToken, DateTime? refreshTokenExpiryTime);
    Task<int> CreateUser(UserForCreationDto userForCreationDto, int userId);
    Task AddUserRoles(int userRole, int id);
    Task<UserDto> GetById(int id);
    Task<int> GetIdByUUID(string uuid);
    Task UpdatePassword(int id, string password);
    Task<int> UpdateUser(UserForCreationDto userForCreationDto, int id, int updatedBy);
    Task DeleteUserRoles(int id);
    Task<IEnumerable<UserForAllDto>> GetAllUsers(UsersAllParameters parameters);
    Task Delete(int id, int userId);
}