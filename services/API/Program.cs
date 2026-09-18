using API.Data;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
