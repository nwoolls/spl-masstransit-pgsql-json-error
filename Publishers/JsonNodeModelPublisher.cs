using System.Text.Json.Nodes;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using spl_masstransit_pgsql_json_error.Contracts;

namespace spl_masstransit_pgsql_json_error.Publishers;

internal sealed class JsonNodeModelPublisher(
    ILogger<JsonNodeModelPublisher> logger,
    IPublishEndpoint publishEndpoint) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Publishing JsonNodeContract");

        const string json = """
                            {
                                "foo": "bar",
                                "fizz": "buzz"
                            }
                            """;
        var jsonNode = JsonNode.Parse(json);
        var contract = new JsonNodeContract(jsonNode!);
        await publishEndpoint.Publish(contract, stoppingToken);
    }
}