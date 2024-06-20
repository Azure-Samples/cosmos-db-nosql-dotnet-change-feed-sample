namespace Sample.ChangeFeed.Console.Options;

public record CosmosDbOptions()
{
    public required string EndpointUri { get; init; }

    public required string DatabaseName { get; init; }

    public required string ContainerName { get; init; }

    public required string LeaseContainerName { get; init; }
}