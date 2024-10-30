using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using spl_masstransit_pgsql_json_error.Contracts;

namespace spl_masstransit_pgsql_json_error.Consumers;

internal sealed class ManagedObjectModelConsumer(ILogger<ManagedObjectModelConsumer> logger) : IConsumer<ManagedObjectContract>
{
    public Task Consume(ConsumeContext<ManagedObjectContract> context)
    {
        var managedObjectModel = context.Message;
        logger.LogInformation("Received ManagedObjectModel: {ManagedObjectModel}", JsonSerializer.Serialize(managedObjectModel));
        return Task.CompletedTask;
    }
}