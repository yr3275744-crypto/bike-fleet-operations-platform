using API.Data;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
var mySqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
// Add services to the container.
builder.Services.AddSingleton(sp => new ConfigStrings
{
    MySqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")!,
    MongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")!,
    MongoDbName = Environment.GetEnvironmentVariable("MONGO_DB_NAME")!,
    MongoCollectionName = Environment.GetEnvironmentVariable("MONGO_COLLECTION_NAME")!,
    RedisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(o => o
    .UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));
builder.Services.AddScoped<IStationsService, StationsService>();

var app = builder.Build();

// enshure db exists
using (var scope = app.Services.CreateScope())
{
    int retries = 0;
    while (retries < 10)
    {
        try
        {
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            break;
        }
        catch (MySqlException)
        {
            retries++;
            await Task.Delay(3000);
        }
    }

}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
