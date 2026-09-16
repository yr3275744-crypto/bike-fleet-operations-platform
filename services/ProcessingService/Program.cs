using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector.Logging;
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
                GroupId = Environment.GetEnvironmentVariable("GROUP_ID")!,
                //StationInformationGroup = Environment.GetEnvironmentVariable("GROUP_ID_STATION_INFORMATION")!,
                //StationStatusGroup = Environment.GetEnvironmentVariable("GROUP_ID_STATION_STATUS")!,
                //VehicleTypesGroup = Environment.GetEnvironmentVariable("GROUP_ID_VEHICLE_TYPES")!,
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
            serviceCollection.AddSingleton<StationInformationHandler>();
            serviceCollection.AddSingleton<VehicleTypesHandler>();
            serviceCollection.AddSingleton<ConsumeManager>();
            //serviceCollection.AddSingleton(_ => new ConsoleLoggerProvider().CreateLogger("proccessor"));
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            //var scope = serviceProvider.CreateScope();
            using (var scope = serviceProvider.CreateScope())
            {
                serviceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();
            }

           await serviceProvider.GetRequiredService<ConsumeManager>().ConsumeLoop();
        }
    }
}
