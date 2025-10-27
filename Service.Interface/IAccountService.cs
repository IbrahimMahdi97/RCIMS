using Shared.DataTransferObjects;

namespace Service.Interface;

public interface IAccountService
{
    Task<int> Create(AccountForManipulationDto account, int userId);
    Task<IEnumerable<AccountDto>> GetAll();
    Task<AccountDto> GetById(string id);
    Task Update(AccountForManipulationDto account, string id, int userId);
    Task Delete(string id);
    Task<AccountDto> GetByNameType(string name, int type);
}