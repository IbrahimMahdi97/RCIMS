using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

internal sealed class AssetRepository : IAssetRepository
{
    private readonly DapperContext _context;

    public AssetRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> Create(AssetForManipulationDto asset, int userId)
    {
        const string query = AssetQuery.InsertQuery;
        var param = new DynamicParameters(asset);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, param);
        return id;
    }

    public async Task<IEnumerable<AssetDto>> GetAll()
    {
        const string query = AssetQuery.GetAllQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<AssetDto>(query);
        return result;
    }

    public async Task<AssetDto> GetById(string id)
    {
        const string query = AssetQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<AssetDto>(query, new {Id = id});
        return result;
    }

    public async Task Update(AssetForManipulationDto asset, string id, int userId)
    {
        const string query = AssetQuery.UpdateQuery;
        var param = new DynamicParameters(asset);
        param.Add("UserId", userId);
        param.Add("Id", id);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }

    public async Task Delete(string id)
    {
        const string query = AssetQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new {Id = id});
    }

    public async Task<int> GetIdByUUID(string uuid)
    {
        const string query = AssetQuery.GetIdByUUIDQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<int>(query, new {Id = uuid});
        return result;
    }
}