using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using IngestionService.Models;
namespace IngestionService;
class Program
{
    public async Task main()
    {
        DotNetEnv.Env.Load();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton(sp => new configStrings
        {
            BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS")!,
            StationInformationTopic = Environment.GetEnvironmentVariable("STATION_INFORMATION_TOPIC")!,
            StationStatusTopic = Environment.GetEnvironmentVariable("STATION_STATUS_TOPIC")!,
            VehicleTypesTopic = Environment.GetEnvironmentVariable("VEHICLE_TYPES_TOPIC")!,
            StationInformationUrl = Environment.GetEnvironmentVariable("STATION_INFORMATION_URL")!,
            StationStatusUrl = Environment.GetEnvironmentVariable("STATION_STATUS_URL")!,
            VehicleTypesUrl = Environment.GetEnvironmentVariable("VEHICLE_TYPES_URL")!
        });
    }
}