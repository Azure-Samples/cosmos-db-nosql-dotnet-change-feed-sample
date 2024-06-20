using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.ChangeFeed.Console.Models;
using Sample.ChangeFeed.Console.Options;

namespace Sample.ChangeFeed.Console.Services;

internal class ConsumerService(ILogger<ConsumerService> logger, IOptions<CosmosDbOptions> options, CosmosClient client) : IHostedService
{
    private readonly CosmosDbOptions _options = options.Value;

    private ChangeFeedProcessor? processor { get; set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("[Starting Change Feed Processor...]");

        Database database = client.GetDatabase(
            id: _options.DatabaseName
        );

        Container dataContainer = database.GetContainer(
            id: _options.ContainerName
        );

        Container leaseContainer = database.GetContainer(
            id: _options.LeaseContainerName
        );

        ChangeFeedProcessorBuilder builder = dataContainer.GetChangeFeedProcessorBuilder<Item>(
            processorName: "data-processor",
            onChangesDelegate: HandleChangesAsync
        );

        processor = builder
            .WithInstanceName("console-instance")
            .WithLeaseContainer(leaseContainer)
            .Build();

        await processor.StartAsync();

        logger.LogInformation("[Waiting for changes...]");
    }

    private async Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<Item> items, CancellationToken cancellationToken)
    {
        foreach (Item item in items)
        {
            logger.LogInformation($"[Change Found] - {item.Id}");
        }

        await Task.Delay(TimeSpan.FromSeconds(0.5)); // Simulate asynchronous operation
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("[Stopping Change Feed Processor...]");

        if (processor is not null)
        {
            await processor.StopAsync();
        }
    }
}