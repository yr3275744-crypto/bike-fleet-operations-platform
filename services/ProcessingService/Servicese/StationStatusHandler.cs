using MongoDB.Driver;
using ProcessingService.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProcessingService.Servicese
{
    public class StationStatusHandler
    {
        private readonly IMongoCollection<StationStatusMongoDto> _collection;
        //private readonly ConnectionMultiplexer _redis
        private readonly IDatabase _db;
        public StationStatusHandler(ConfigStrings configStrings)
        {
            _collection = new MongoClient(configStrings.MongoConnectionString)
                .GetDatabase(configStrings.MongoDbName)
                .GetCollection<StationStatusMongoDto>(configStrings.MongoCollectionName);
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configStrings.RedisConnectionString);
            _db = redis.GetDatabase();
        }
        public async Task CreateAsync(StationStatusMongoDto stationStatus)
        {
            await _collection.InsertOneAsync(stationStatus);
        }
        public async Task<bool> CompareWithRedis(string key, string value)
        {
            string? exists = _db.StringGet(key);
            if (exists == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> AddToRedis(string key, string value)
        {
            return _db.StringSet(key, value);
        }
        public async Task CreateManagmentAsync(StationStatusMongoDto stationStatus)
        {
            string stringStation = JsonSerializer.Serialize(stationStatus);
            string key = stationStatus.StationId;
            bool isInRedis = await CompareWithRedis(key, stringStation);
            if (isInRedis)
            {
                return;
            }
            else
            {
                await AddToRedis(key, stringStation);
                await CreateAsync(stationStatus);
            }
        }
    }

}
