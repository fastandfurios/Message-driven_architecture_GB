using FluentMigrator.Runner;
using MassTransit;
using MassTransit.Audit;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Kitchen.Consumers;
using Restaurant.Kitchen.Exceptions;

namespace Restaurant.Kitchen.Extensions
{
    internal static class ServicesEx
    {
        /// <summary> Adding and configuring mass transit. </summary>
        /// <param name="services">service collection interface</param>
        internal static void AddAndConfigMassTransit(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var auditStore = serviceProvider.GetService<IMessageAuditStore>();

            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<KitchenTableBookedConsumer>(cfg =>
                {
                    cfg.UseScheduledRedelivery(config =>
                    {
                        config.Interval(1, TimeSpan.FromSeconds(2));
                    });
                    cfg.UseMessageRetry(config =>
                    {
                        config.Incremental(retryLimit: 3, initialInterval: TimeSpan.FromSeconds(1),
                            intervalIncrement: TimeSpan.FromSeconds(2));
                        config.Handle<LasagnaException>();
                    });
                });

                configure.AddConsumer<KitchenTableBookedConsumer>();

                configure.AddConsumer<KitchenFaultConsumer>();

                configure.AddDelayedMessageScheduler();

                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);
                    cfg.ConnectSendAuditObservers(auditStore);
                    cfg.ConnectConsumeAuditObserver(auditStore);
                });
            });
        }

        /// <summary> Configures SqLite </summary>
        /// <param name="services">service collection interface</param>
        /// <param name="connectionString">connection string</param>
        internal static void SqLiteConfiguring(this IServiceCollection services, string connectionString)
        {
            var service = services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg.AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(cfg => cfg.AddFluentMigratorConsole())
                .BuildServiceProvider(false);

            var runner = service.CreateScope().ServiceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }
    }
}
