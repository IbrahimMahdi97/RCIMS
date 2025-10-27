using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

internal sealed class InstallmentRepository : IInstallmentRepository
{
    private readonly DapperContext _context;

    public InstallmentRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> Create(InstallmentForManipulationDto installment, int userId)
    {
        const string query = InstallmentQuery.InsertQuery;
        var param = new DynamicParameters(installment);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, param);
        return id;
    }

    public async Task<IEnumerable<InstallmentDto>> GetAll()
    {
        const string query = InstallmentQuery.GetAllQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<InstallmentDto>(query);
        return result;
    }

    public async Task<InstallmentDto> GetById(string id)
    {
        const string query = InstallmentQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<InstallmentDto>(query, new {Id = id});
        return result;
    }

    public async Task Update(InstallmentForManipulationDto installment, string id, int userId)
    {
        const string query = InstallmentQuery.UpdateQuery;
        var param = new DynamicParameters(installment);
        param.Add("UserId", userId);
        param.Add("Id", id);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }

    public async Task Delete(string id)
    {
        const string query = InstallmentQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new {Id = id});
    }
}