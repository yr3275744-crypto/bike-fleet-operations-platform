using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProcessingService.Data;
using ProcessingService.Models;
using ProcessingService.Servicese;

namespace ProcessingService
{
    class Program
    {
        public static async Task Main()
        {
            DotNetEnv.Env.Load();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(sp => new ConfigStrings
            {
                BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS")!,
                StationInformationTopic = Environment.GetEnvironmentVariable("STATION_INFORMATION_TOPIC")!,
                StationStatusTopic = Environment.GetEnvironmentVariable("STATION_STATUS_TOPIC")!,
                VehicleTypesTopic = Environment.GetEnvironmentVariable("VEHICLE_TYPES_TOPIC")!,
                StationInformationGroup = Environment.GetEnvironmentVariable("GROUP_ID_STATION_INFORMATION")!,
                StationStatusGroup = Environment.GetEnvironmentVariable("GROUP_ID_STATION_STATUS")!,
                VehicleTypesGroup = Environment.GetEnvironmentVariable("GROUP_ID_VEHICLE_TYPES")!,
                MySqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")!,
                MongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")!,
                MongoCollectionName = Environment.GetEnvironmentVariable("MONGO_COLLECTION_NAME")!,
                MongoDbName = Environment.GetEnvironmentVariable("MONGO_DB_NAME")!
            });
            var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")
                ?? throw new InvalidOperationException("Connection string"
            + "'DefaultConnection' not found.");

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            serviceCollection.AddSingleton<StationStatusHandler>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Console.WriteLine(await serviceProvider.GetRequiredService<StationStatusHandler>().GetAsync() != null);
        }
    }
}
