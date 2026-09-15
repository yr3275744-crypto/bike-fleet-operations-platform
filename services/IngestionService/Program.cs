using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using IngestionService.Models;
using IngestionService.services;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
namespace IngestionService;
class Program
{
    public async static Task Main()
    {
        DotNetEnv.Env.Load();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton(sp => new ConfigStrings
        {
            BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS")!,
            StationInformationTopic = Environment.GetEnvironmentVariable("STATION_INFORMATION_TOPIC")!,
            StationStatusTopic = Environment.GetEnvironmentVariable("STATION_STATUS_TOPIC")!,
            VehicleTypesTopic = Environment.GetEnvironmentVariable("VEHICLE_TYPES_TOPIC")!,
            StationInformationUrl = Environment.GetEnvironmentVariable("STATION_INFORMATION_URL")!,
            StationStatusUrl = Environment.GetEnvironmentVariable("STATION_STATUS_URL")!,
            VehicleTypesUrl = Environment.GetEnvironmentVariable("VEHICLE_TYPES_URL")!
        });
        serviceCollection.AddLogging(logging =>
            logging.AddConsole());
        serviceCollection.AddSingleton<StationInformationService>();
        serviceCollection.AddSingleton<StationStatusService>();
        serviceCollection.AddSingleton<VehicleTypesService>();
        serviceCollection.AddSingleton<ProducerService>();
        serviceCollection.AddSingleton<Orchestrator>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        await serviceProvider.GetRequiredService<Orchestrator>().Play();
    }
}