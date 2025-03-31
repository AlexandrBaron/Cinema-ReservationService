using System.Reflection;
using CreateUserCommand = RegistrationService.UseCases.Users.Create.CreateUserCommand;
using User = RegistrationService.Core.Entities.User;


namespace RegistrationService.API.Configurations;
public static class MediatrConfigs
{
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        var mediatRAssemblies = new[]
          {
        Assembly.GetAssembly(typeof(User)), // Core
        Assembly.GetAssembly(typeof(CreateUserCommand)) // UseCases
      };

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        return services;
    }
}
