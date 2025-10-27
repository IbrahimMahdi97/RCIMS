using Dapper;
using Interface;
using Repository.Query;
using Shared.DataTransferObjects;
using Shared.Helpers;
using Shared.Parameters;

namespace Repository;

internal sealed class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<UserDto?> FindByCredentialsUsername(string username, string password)
    {
        const string query = UserQuery.UserByCredentialsUsernameQuery;
        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(query,
            new { Username = username, Password = password });
        return user;
    }
    public async Task<IEnumerable<UserRoleDto>> GetUserRoles(int userId)
    {
        const string query = UserQuery.UserRolesByUserIdQuery;
        using var connection = _context.CreateConnection();
        var roles = await connection.QueryAsync<UserRoleDto>(query, new { Id = userId });
        return roles.ToList();
    }
    public async Task<int> FindIdByUsername(string username)
    {
        const string query = UserQuery.UserIdByUsernameQuery;
        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<int>(query,
            new { Username = username });
        return user;
    }
    public async Task UpdateRefreshToken(int id, string refreshToken, DateTime? refreshTokenExpiryTime)
    {
        const string query = UserQuery.UpdateRefreshTokenByIdQuery;
        using var connection = _context.CreateConnection();
        connection.Open();
        using var trans = connection.BeginTransaction();
        await connection.ExecuteAsync(query,
            new { Id = id, RefreshToken = refreshToken, RefreshTokenExpiryTime = refreshTokenExpiryTime },
            transaction: trans);
        trans.Commit();
    }
    public async Task<int> CreateUser(UserForCreationDto userForCreationDto, int creatorId)
    {
        const string query = UserQuery.CreateUserQuery;
        var param = new DynamicParameters(userForCreationDto);
        param.Add("UserId", creatorId);
        using var connection = _context.CreateConnection();
        connection.Open();
        using var trans = connection.BeginTransaction();
        var id = await connection.QuerySingleAsync<int>(query, param, transaction: trans);
        const string passwordQuery = UserQuery.AddEncryptedPasswordByIdQuery;
        string encryptedPassword = (userForCreationDto.Password + id).ToSha512();
        await connection.ExecuteAsync(passwordQuery, new { Password = encryptedPassword, Id = id }, transaction: trans);
        trans.Commit();
        return id;
    }
    public async Task AddUserRoles(int userRole, int id)
    { 
        const string query = RoleQuery.InsertUserRolesQuery;
        using var connection = _context.CreateConnection();
        connection.Open();
        var param = new DynamicParameters();
        param.Add("RoleId", userRole);
        param.Add("UserId", id);
        await connection.ExecuteAsync(query, param);
    }
    public async Task<UserDto> GetById(int id)
    {
        const string query = UserQuery.GetByIdQuery;
        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(query,
            new { Id = id });
        return user;
    }
    public async Task<int> GetIdByUUID(string uuid)
    {
        const string query = UserQuery.UserIdByUUIDQuery;
        using var connection = _context.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<int>(query, new { UUID = uuid});
        return result;
    }
    public async Task UpdatePassword(int id, string password)
    {
        const string query = UserQuery.AddEncryptedPasswordByIdQuery;
        using var connection = _context.CreateConnection();
        connection.Open();
        using var trans = connection.BeginTransaction();
        await connection.ExecuteAsync(query, new { Password = password, Id = id }, transaction: trans);
        trans.Commit();
    }
    public async Task<int> UpdateUser(UserForCreationDto userForCreationDto, int id, int updatedBy)
    {
        const string query = UserQuery.UpdateUserQuery;
        var param = new DynamicParameters(userForCreationDto);
        param.Add("UserId", id);
        param.Add("UpdatedBy", updatedBy);
        using var connection = _context.CreateConnection();
        connection.Open();
        var userId = await connection.QuerySingleAsync<int>(query, param);
        return userId;
    }
    public async Task DeleteUserRoles(int id)
    {
        const string query = RoleQuery.DeleteUserRolesQuery;
        using var connection = _context.CreateConnection();
        connection.Open();
        await connection.ExecuteAsync(query, new { Id = id});
    }
    public async Task<IEnumerable<UserForAllDto>> GetAllUsers(UsersAllParameters parameters)
    {
        const string query = UserQuery.AllUsersQuery;
        var offset = (parameters.PageNumber - 1) * parameters.PageSize;
        var param = new DynamicParameters(parameters);
        param.Add("Offset", offset);
        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<UserForAllDto>(query, param: param);
        int count = users.Count();
        return users;
    }

    public async Task Delete(int id, int userId)
    {
        const string query = UserQuery.DeleteQuery;
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { Id = id, UserId = userId });
    }
}