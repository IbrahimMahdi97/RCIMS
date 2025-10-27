using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;

namespace Repository;

internal sealed class RoleRepository : IRoleRepository
{
    private readonly DapperContext _context;

    public RoleRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<UserRoleDto>> GetUserRoles(int id)
    {
        const string query = RoleQuery.UserRolesByIdQuery;
        using var connection = _context.CreateConnection();
        var roles = await connection.QueryAsync<UserRoleDto>(query, new { Id = id });
        return roles;
    }

    public async Task<IEnumerable<UserRoleDto>> GetAll()
    {
        const string query = RoleQuery.AllQuery;
        using var connection = _context.CreateConnection();
        var roles = await connection.QueryAsync<UserRoleDto>(query);
        return roles;
    }
    public async Task<int> Create(RoleForManipulationDto roleForCreationDto, int creatorId)
    {
        const string query = RoleQuery.CreateQuery;
        var param = new DynamicParameters(roleForCreationDto);
        param.Add("UserId", creatorId);
        using var connection = _context.CreateConnection();
        connection.Open();
        using var trans = connection.BeginTransaction();
        var id = await connection.QuerySingleAsync<int>(query, param, transaction: trans);
        trans.Commit();
        return id;
    }
    public async Task<UserRoleDto> GetById(int id)
    {
        const string query = RoleQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var role = await connection.QueryFirstOrDefaultAsync<UserRoleDto>(query, new { Id = id });
        return role;
    }
    public async Task Update(RoleForManipulationDto roleDto, int id, int userId)
    {
        const string query = RoleQuery.UpdateQuery;
        var param = new DynamicParameters(roleDto);
        param.Add("Id", id);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }
    public async Task Delete(int id,int userId)
    {
        const string query = RoleQuery.DeleteQuery;
        var param = new DynamicParameters();
        param.Add("Id", id);
        param.Add("UserId", userId);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, param);
    }
}