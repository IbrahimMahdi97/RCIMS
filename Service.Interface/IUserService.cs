using Shared.DataTransferObjects;
using Shared.Parameters;

namespace Service.Interface;

public interface IUserService
{
    Task<UserDto> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<int> CreateUser(UserForCreationDto userForCreationDto, int creatorId);
    Task<UserDto> GetById(string id);
    Task<UserDto> GetMyDetails(int id);
    Task UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto);
    Task UpdateUser(UserForCreationDto userForCreationDto, string id, int updatedBy);
    Task<PagedList<UserForAllDto>> GetAll(UsersAllParameters parameters);
    Task Delete(string id, int userId);
}