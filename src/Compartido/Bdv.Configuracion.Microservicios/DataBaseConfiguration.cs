namespace Bdv.Configuracion.Microservicios
{
    public static class DataBaseConfiguration
    {
        internal static IServiceCollection ConfigureDatabase<TContext>(this IServiceCollection services,
            IConfiguration configuration,
            bool esQuery,
            DatabaseType databaseType) where TContext : DbContext
        {
            switch (databaseType)
            {
                case DatabaseType.SQLServer:
                    services.ConfiguraDatabaseSqlServer<TContext>(configuration, esQuery);
                    break;
                case DatabaseType.MySQL:
                    services.ConfiguraDatabaseMySql<TContext>(configuration, esQuery);
                    break;
            }

            return services;
        }

        private static IServiceCollection ConfiguraDatabaseSqlServer<TContext>(this IServiceCollection services,
            IConfiguration configuration,
            bool esQuery) where TContext : DbContext
        {
            if (esQuery)
            {
                services.AddDbContext<TContext>(options =>
                    options
                    .UseSqlServer(configuration.GetConnectionString("ConnectionStringRead"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }
            else
            {
                services.AddScoped<UpdateAddedInterceptor>();
                services.AddScoped<OutboxInterceptor>();

                services.AddDbContext<TContext>((sp, options) =>
                {
                    options
                    .UseSqlServer(configuration.GetConnectionString("ConnectionString"), sqlServerOptionsAction =>
                    {
                        sqlServerOptionsAction.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: Array.Empty<int>());
                        sqlServerOptionsAction.CommandTimeout(30);
                    })
                    .AddInterceptors(sp.GetRequiredService<UpdateAddedInterceptor>(),
                                     sp.GetRequiredService<OutboxInterceptor>());
                });
            }

            return services;
        }
        private static IServiceCollection ConfiguraDatabaseMySql<TContext>(this IServiceCollection services,
            IConfiguration configuration,
            bool esQuery) where TContext : DbContext
        {
            if (esQuery)
            {
                var mySqlConnectionStr = configuration.GetConnectionString("ConnectionStringRead");
                services.AddDbContext<TContext>(options =>
                    options
                    .UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }
            else
            {
                var mySqlConnectionStr = configuration.GetConnectionString("ConnectionString");
                services.AddScoped<UpdateAddedInterceptor>();
                services.AddScoped<AuditingInterceptor>();
                services.AddScoped<OutboxInterceptor>();

                services.AddDbContext<TContext>((sp, options) =>
                {
                    options
                    .UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr),
                    mySqlOptionsAction =>
                    {
                        mySqlOptionsAction.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: Array.Empty<int>());
                        mySqlOptionsAction.CommandTimeout(30);
                    })
                    .AddInterceptors(sp.GetRequiredService<UpdateAddedInterceptor>(),
                                     sp.GetRequiredService<AuditingInterceptor>(),
                                     sp.GetRequiredService<OutboxInterceptor>());
                });
            }

            return services;
        }
    }
}