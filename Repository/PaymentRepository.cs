using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly DapperContext _context;

    public PaymentRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> Create(PaymentForManipulationDto payment, int userId)
    {
        const string query = PaymentQuery.InsertQuery;
        var param = new DynamicParameters(payment);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, param);
        return id;
    }

    public async Task<IEnumerable<PaymentDto>> GetAll()
    {
        const string query = PaymentQuery.GetAllQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<PaymentDto>(query);
        return result;
    }

    public async Task<PaymentDto> GetById(string id)
    {
        const string query = PaymentQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<PaymentDto>(query, new {Id = id});
        return result;
    }

    public async Task Update(PaymentForManipulationDto payment, string id, int userId)
    {
        const string query = PaymentQuery.UpdateQuery;
        var param = new DynamicParameters(payment);
        param.Add("UserId", userId);
        param.Add("Id", id);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }

    public async Task Delete(string id)
    {
        const string query = PaymentQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new {Id = id});
    }
}