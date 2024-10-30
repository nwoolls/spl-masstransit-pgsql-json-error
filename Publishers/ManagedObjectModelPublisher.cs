using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using spl_masstransit_pgsql_json_error.Contracts;

namespace spl_masstransit_pgsql_json_error.Publishers;

internal sealed class ManagedObjectModelPublisher(
    ILogger<ManagedObjectModelPublisher> logger,
    IPublishEndpoint publishEndpoint) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Publishing ManagedObjectContract");

        const string json = """
                            {
                                "foo": "bar",
                                "fizz": "buzz"
                            }
                            """;
        var model = JsonSerializer.Deserialize<ManagedObjectModel>(json, JsonSerializerOptions.Default);
        var contract = new ManagedObjectContract(model!);
        await publishEndpoint.Publish(contract, stoppingToken);
    }
}