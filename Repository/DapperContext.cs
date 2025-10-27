using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Repository;

public class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_configuration
            .GetConnectionString("postgresConnection"));

    public IDbConnection CreateMasterConnection()
        => new NpgsqlConnection(_configuration
            .GetConnectionString("masterPostgresConnection"));
}