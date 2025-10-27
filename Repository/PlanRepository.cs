using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

internal sealed class PlanRepository : IPlanRepository
{
    private readonly DapperContext _context;

    public PlanRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> Create(PlanForManipulationDto plan, int userId)
    {
        const string query = PlanQuery.InsertQuery;
        var param = new DynamicParameters(plan);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, param);
        return id;
    }

    public async Task<IEnumerable<PlanDto>> GetAll()
    {
        const string query = PlanQuery.GetAllQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<PlanDto>(query);
        return result;
    }

    public async Task<PlanDto> GetById(string id)
    {
        const string query = PlanQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<PlanDto>(query, new {Id = id});
        return result;
    }

    public async Task Update(PlanForManipulationDto plan, string id, int userId)
    {
        const string query = PlanQuery.UpdateQuery;
        var param = new DynamicParameters(plan);
        param.Add("UserId", userId);
        param.Add("Id", id);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }

    public async Task Delete(string id)
    {
        const string query = PlanQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new {Id = id});
    }

    public async Task<PlanDto> GetByName(string name)
    {
        const string query = PlanQuery.GetByNameQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<PlanDto>(query, new { Name = name });
        return result;
    }

    public async Task<int> GetIdByUUID(string uuid)
    {
        const string query = PlanQuery.GetIdByUUIDQuery;
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleOrDefaultAsync<int>(query, new { Uuid = uuid });
        return id;
    }
}