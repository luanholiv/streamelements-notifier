using Api.Database.Stores;

namespace Api.Database;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("Database");

        services.AddNpgsqlDataSource(dbConnectionString!);

        services.AddSingleton<IStoreRepository, StoreRepository>();

        return services;
    }
}
