using API.Data;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Text.Json;

namespace API.Services
{
    public class StationsService : IStationsService
    {
        private readonly IDatabase _redisDb;
        private readonly IMongoCollection<StationStatusMongoDto> _mongoCollection;
        private readonly ApplicationDbContext _dbContext;
        public StationsService(ConfigStrings configStrings,
            ApplicationDbContext applicationDbContext)
        {
            var redis = ConnectionMultiplexer.Connect(configStrings.RedisConnectionString);
            _redisDb = redis.GetDatabase();
            _dbContext = applicationDbContext;

            _mongoCollection = new MongoClient(configStrings.MongoConnectionString)
                .GetDatabase(configStrings.MongoDbName)
                .GetCollection<StationStatusMongoDto>(configStrings.MongoCollectionName);
        }
        public async Task<IEnumerable<GetStationsResponseDto>> GetStations()
        {
            List<GetStationsResponseDto> dtos = new();
            var info = await _dbContext.StationInformations.ToListAsync();
            foreach (StationInformation station in info)
            {
                string? statusString = _redisDb.StringGet(station.StationId);
                if (string.IsNullOrEmpty(statusString))
                {
                    Console.WriteLine("status of station not found");
                    continue;
                }
                var status = JsonSerializer.Deserialize<StationStatusMongoDto>(statusString);
                if (status == null)
                {
                    Console.WriteLine("seralize status faild");
                    continue;
                }
                dtos.Add(new GetStationsResponseDto
                {
                    StationId = station.StationId,
                    Latitude = station.Latitude,
                    Longitude = station.Longitude,
                    Capacity = station.Capacity,
                    Name = station.Name,
                    IsRenting = status.IsRenting,
                    IsReturning = status.IsReturning,
                    NumBikesAvailable = status.NumBikesAvailable,
                    NumDocksAvailable = status.NumDocksAvailable
                });
            }
            return dtos;
        }

    }
}
