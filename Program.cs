using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using spl_masstransit_pgsql_json_error.Consumers;
using spl_masstransit_pgsql_json_error.Publishers;

HostApplicationBuilderSettings settings = new()
{
    Args = args,
    Configuration = new ConfigurationManager(),
    ContentRootPath = Directory.GetCurrentDirectory(),
};

settings.Configuration.AddJsonFile("hostsettings.json", optional: true);
settings.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
settings.Configuration.AddCommandLine(args);

var builder = Host.CreateApplicationBuilder(settings);

builder.Services.AddMassTransit(busRegistrationConfigurator =>
{
    busRegistrationConfigurator.AddConsumer<JsonNodeModelConsumer>();
    busRegistrationConfigurator.AddConsumer<ManagedObjectModelConsumer>();
    busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();

    var connectionString = builder.Configuration.GetConnectionString("messaging-db");
    builder.Services.AddOptions<SqlTransportOptions>()
        .Configure(options => options.ConnectionString = connectionString);
    builder.Services.AddPostgresMigrationHostedService();

    busRegistrationConfigurator.AddSqlMessageScheduler();
    busRegistrationConfigurator.UsingPostgres((busRegistrationContext, sqlBusFactoryConfigurator) =>
    {
        sqlBusFactoryConfigurator.UseSqlMessageScheduler();
        sqlBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
    });
});

// Works:
builder.Services.AddHostedService<JsonNodeModelPublisher>();

// Fails w/ "Npgsql.PostgresException (0x80004005): 22P02: invalid input syntax for type json":
builder.Services.AddHostedService<ManagedObjectModelPublisher>();

var host = builder.Build();

await host.RunAsync();