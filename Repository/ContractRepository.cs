using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

public class ContractRepository : IContractRepository
{
    private readonly DapperContext _context;

    public ContractRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> Create(ContractForManipulationDto contract, int userId)
    {
        const string query = ContractQuery.InsertQuery;
        var param = new DynamicParameters(contract);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, param);
        return id;
    }

    public async Task<IEnumerable<ContractDto>> GetAll()
    {
        const string query = ContractQuery.GetAllQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<ContractDto>(query);
        return result;
    }

    public async Task<ContractDto> GetById(string id)
    {
        const string query = ContractQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<ContractDto>(query, new {Id = id});
        return result;
    }

    public async Task Update(ContractForManipulationDto contract, string id, int userId)
    {
        const string query = ContractQuery.UpdateQuery;
        var param = new DynamicParameters(contract);
        param.Add("UserId", userId);
        param.Add("Id", id);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }

    public async Task Delete(string id)
    {
        const string query = ContractQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new {Id = id});
    }

    public async Task<string> GetLastContractNumber()
    {
        const string query = ContractQuery.GetLastContractNumberQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<string>(query);
        return result;
    }
}