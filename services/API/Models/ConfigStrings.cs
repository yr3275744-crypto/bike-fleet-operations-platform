using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class ConfigStrings
    {
        public string MySqlConnectionString { get; set; } = string.Empty;
        public string MongoConnectionString { get; set; } = string.Empty;
        public string MongoDbName { get; set; } = string.Empty;
        public string MongoCollectionName { get; set; } = string.Empty;
        public string RedisConnectionString { get; set; } = string.Empty;
    }
}
