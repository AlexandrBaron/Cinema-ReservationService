using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Quartz;
using RegistrationService.API.Configurations;
using RegistrationService.Infrastructure.BackgroundJob;
using RegistrationService.Infrastructure.Client;
using RegistrationService.Infrastructure.RabbitMQ;
using RegistrationService.Infrastructure.TokenService;
using RegistrationService.Shared;

namespace RegistrationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

            logger.Information("Starting api");

            builder.AddLoggerConfigs();

            builder.Services.AddSingleton<TokenService>();

            builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

            builder.Services.AddHttpClient<CinemaServiceHttpClient>();
            builder.Services.AddHttpClient<MovieServiceHttpClient>();
            builder.Services.Configure<ReservationOptions>(
            builder.Configuration.GetSection("Reservation"));


            builder.Services.AddScoped<DeleteExpiredReservations>();

            builder.Services.AddQuartz(q =>
            {
                q.AddJob<DeleteExpiredReservationsJob>(job =>
                {
                    job.WithIdentity("DeleteExpiredReservationsJob");

                });
                q.AddTrigger(trigger =>
                {
                    trigger.ForJob("DeleteExpiredReservationsJob")
                           .WithIdentity("DeleteExpiredReservationsTrigger")
                           .WithCronSchedule("0 0 0 1/1 * ? *");
                });
            });
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            builder.Services.Configure<ReservationSettings>(builder.Configuration.GetSection("ReservationSettings"));

            builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
            builder.Services.AddSingleton<RabbitMQManager>(sp =>
            {
                var rabbitMqSettings = sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

                return new RabbitMQManager(
                    rabbitMqSettings.HostName,
                    rabbitMqSettings.NotificationQueueName,
                    rabbitMqSettings.NotificationExchangeName,
                    sp.GetRequiredService<ILogger<RabbitMQManager>>()
                );
            });


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            var appLoger = new SerilogLoggerFactory(logger)
                .CreateLogger<Program>();

            builder.Services.AddOptionConfigs(builder.Configuration, appLoger, builder);
            builder.Services.AddServiceConfigs(appLoger, builder);

            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                    o.ShortSchemaNames = true;
                });

            var app = builder.Build();

            app.UseAppMiddlewareAndSeedDatabase();

            app.Run();


        }
    }
}
