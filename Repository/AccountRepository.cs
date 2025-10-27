using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

internal sealed class AccountRepository : IAccountRepository
{
    private readonly DapperContext _context;

    public AccountRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> Create(AccountForManipulationDto account, int userId)
    {
        const string query = AccountQuery.InsertQuery;
        var param = new DynamicParameters(account);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, param);
        return id;
    }

    public async Task<IEnumerable<AccountDto>> GetAll()
    {
        const string query = AccountQuery.GetAllQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<AccountDto>(query);
        return result;
    }

    public async Task<AccountDto> GetById(string id)
    {
        const string query = AccountQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<AccountDto>(query, new {Id = id});
        return result;
    }

    public async Task Update(AccountForManipulationDto account, string id, int userId)
    {
        const string query = AccountQuery.UpdateQuery;
        var param = new DynamicParameters(account);
        param.Add("UserId", userId);
        param.Add("Id", id);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }

    public async Task Delete(string id)
    {
        const string query = AccountQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new {Id = id});
    }

    public async Task<AccountDto> GetByNameType(string name, int type)
    {
        const string query = AccountQuery.GetByNameTypeQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<AccountDto>(query, new { Name = name, Type =  type});
        return result;
    }
}