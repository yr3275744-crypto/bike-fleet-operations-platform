using MongoDB.Driver;
using ProcessingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingService.Servicese
{
    public class StationStatusHandler
    {
        private readonly IMongoCollection<StationStatusMongoDto> _collection;
        public StationStatusHandler(ConfigStrings configStrings)
        {
            _collection = new MongoClient(configStrings.MongoConnectionString)
                .GetDatabase(configStrings.MongoDbName)
                .GetCollection<StationStatusMongoDto>(configStrings.MongoCollectionName);
        }
        public async Task CreateAsync(StationStatusMongoDto stationStatus)
        {
            await _collection.InsertOneAsync(stationStatus);
        }
    }

}
