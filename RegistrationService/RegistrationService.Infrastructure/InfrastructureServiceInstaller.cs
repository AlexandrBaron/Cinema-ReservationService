using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegistrationService.Infrastructure.Repositories;

namespace RegistrationService.Infrastructure
{
    public static class InfrastructureServiceInstaller
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            ConfigurationManager config,
            ILogger logger)
        {
            services.AddDbContextFactory<AppDbContext, ApDbContextFactory>();
            services.AddScoped(sp => sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))

            .AddScoped(typeof(UserRepository))
            .AddScoped(typeof(IRepository<User>), typeof(UserRepository))
            .AddScoped(typeof(IReadRepository<User>), typeof(UserRepository))

            .AddScoped(typeof(ReservationRepository))
            .AddScoped(typeof(IRepository<Reservation>), typeof(ReservationRepository))
            .AddScoped(typeof(IReadRepository<Reservation>), typeof(ReservationRepository));

            logger.LogInformation("{Project} services registered", "Infrastructure");

            return services;
        }
    }
}
