using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sample.ChangeFeed.Console.Options;
using Sample.ChangeFeed.Console.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddOptions<CosmosDbOptions>().BindConfiguration(nameof(CosmosDbOptions));

builder.Services.AddSingleton((IServiceProvider provider) =>
{
    var options = provider.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

    var credential = new DefaultAzureCredential();
    return new CosmosClient(
        accountEndpoint: options.EndpointUri,
        tokenCredential: credential
    );
});

builder.Services.AddHostedService<ConsumerService>();

using IHost host = builder.Build();

await host.RunAsync();