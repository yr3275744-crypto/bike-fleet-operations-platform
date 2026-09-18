using API.Data;
using API.Models;
using API.Services.Interfaces;
using MongoDB.Driver;
using StackExchange.Redis;

namespace API.Services
{
    public class StationsService : IStationsService
    {
        private readonly IDatabase _db;
        private readonly IMongoCollection<StationStatusMongoDto> _mongoCollection;
        private readonly ApplicationDbContext _dbContext;
        public StationsService(ConfigStrings configStrings,
            ApplicationDbContext applicationDbContext)
        {
            var redis = ConnectionMultiplexer.Connect(configStrings.RedisConnectionString);
            _db = redis.GetDatabase();
            _dbContext = applicationDbContext;

            _mongoCollection = new MongoClient(configStrings.MongoConnectionString)
                .GetDatabase(configStrings.MongoDbName)
                .GetCollection<StationStatusMongoDto>(configStrings.MongoCollectionName);
        }
        
    }
}
