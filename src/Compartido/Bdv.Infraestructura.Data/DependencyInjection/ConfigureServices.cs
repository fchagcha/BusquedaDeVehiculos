namespace Bdv.Infraestructura.Data.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfraestructuraServices(this IServiceCollection services, IConfiguration configuration, bool esQuery)
        {
            var assemblies = RegisterServicesFromAssembly(services);

            services.AddHttpContextAccessor()
                    .AddExceptionless(configuration)
                    .AddHttpClient();

            services.AddUnitOfWorkServices(esQuery)
                    .AddMediatRServices(assemblies)
                    .AddBehaviorServices(esQuery, assemblies)
                    .AddMapperService();

            return services;
        }
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionMiddleware>();

            return applicationBuilder;
        }

        private static IServiceCollection AddUnitOfWorkServices(this IServiceCollection services, bool esQuery)
        {
            services.AddScoped<IProvider, Provider>();
            services.AddScoped<IUoWService, UoWService>();

            if (esQuery)
                services.AddScoped<IUoWQuery, UoWQuery>();
            else
                services.AddScoped<IUoWCommand, UoWCommand>();

            return services;
        }
        private static IServiceCollection AddMediatRServices(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
        private static IServiceCollection AddBehaviorServices(this IServiceCollection services, bool esQuery, Assembly[] assemblies)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            if (!esQuery)
                services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblies(assemblies);

            return services;
        }

        private static Assembly[] RegisterServicesFromAssembly(IServiceCollection services)
        {
            var assembliesPath = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories);

            var keywords = new HashSet<string> { "Aplicacion.Command", "Aplicacion.Query" };
            var separator = Path.DirectorySeparatorChar.ToString();
            var runtimesPath = separator + "runtimes" + separator;

            var assemblies =
                assembliesPath
                .Where(path => !path.Contains(runtimesPath))
                .Select(path =>
                {
                    var assemblyName = AssemblyName.GetAssemblyName(path).Name;

                    return keywords.Any(keyword => assemblyName.Contains(keyword))
                        ? AssemblyLoadContext.Default.LoadFromAssemblyPath(path)
                        : null;
                })
                .Where(assembly => assembly != null)
                .ToArray();

            services.RegisterAssemblyPublicNonGenericClasses(assemblies)
                    .AsPublicImplementedInterfaces();

            return assemblies;
        }
    }
}