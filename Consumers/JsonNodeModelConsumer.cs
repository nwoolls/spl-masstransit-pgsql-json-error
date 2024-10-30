using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using spl_masstransit_pgsql_json_error.Contracts;

namespace spl_masstransit_pgsql_json_error.Consumers;

internal sealed class JsonNodeModelConsumer(ILogger<JsonNodeModelConsumer> logger) : IConsumer<JsonNodeContract>
{
    public Task Consume(ConsumeContext<JsonNodeContract> context)
    {
        var jsonNodeModel = context.Message;
        logger.LogInformation("Received JsonNodeModel: {JsonNodeModel}", JsonSerializer.Serialize(jsonNodeModel));
        return Task.CompletedTask;
    }
}