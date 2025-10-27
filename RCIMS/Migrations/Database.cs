using Dapper;
using Repository;

namespace RCIMS.Migrations;

public class Database(DapperContext context)
{
    public void CreateDatabase(string dbName)
    {
        const string query = "SELECT 1 FROM pg_database WHERE datname = @name";

        var parameters = new DynamicParameters();
        parameters.Add("name", dbName);

        using var connection = context.CreateMasterConnection();
        var records = connection.Query(query, parameters);

        if (!records.Any())
            connection.Execute($"CREATE DATABASE \"{dbName}\"");
    }
}