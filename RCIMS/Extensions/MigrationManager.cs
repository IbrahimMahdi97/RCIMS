using FluentMigrator.Runner;
using Interface;
using RCIMS.Migrations;

namespace RCIMS.Extensions;

public static class MigrationManager
{
    public static void MigrateDatabase(this WebApplication app,
        ILoggerManager logger)
    {
        using var scope = app.Services.CreateScope();
        var databaseService = scope.ServiceProvider
            .GetRequiredService<Database>();
        var migrationService = scope.ServiceProvider
            .GetRequiredService<IMigrationRunner>();

        try
        {
            databaseService.CreateDatabase("RCIMS");

            migrationService.ListMigrations();
            migrationService.MigrateUp();
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception occurred during the database creation: {ex}");
            throw;
        }
    }
}