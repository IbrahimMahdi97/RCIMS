using Entities.Exceptions;
using Interface;
using Service.Interface;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class AccountService : IAccountService
{
    private readonly IRepositoryManager _repository;

    public AccountService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<int> Create(AccountForManipulationDto account, int userId)
    {
        var alreadyExistAccount = GetByNameType(account.Name, account.AccountType);
        if (alreadyExistAccount != null)
            throw new AccountAlreadyExistsBadRequestException(account.Name, account.AccountType);
        var result = await _repository.Account.Create(account, userId);
        return result;
    }

    public async Task<IEnumerable<AccountDto>> GetAll()
    {
        var result = await _repository.Account.GetAll();
        return result;
    }

    public async Task<AccountDto> GetById(string id)
    {
        var result = await _repository.Account.GetById(id) ?? throw new AccountNotFoundException();
        return result;
    }

    public async Task Update(AccountForManipulationDto account, string id, int userId)
    {
        var oldAccount = await GetById(id);
        await _repository.Account.Update(account, id, userId);
    }

    public async Task Delete(string id)
    {
        var account = await GetById(id);
        await _repository.Account.Delete(id);
    }

    public async Task<AccountDto> GetByNameType(string name, int type)
    {
        var result = await _repository.Account.GetByNameType(name, type);
        return result;
    }
}