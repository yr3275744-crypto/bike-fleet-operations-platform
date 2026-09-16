using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingService.Models
{
    public class ConfigStrings
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string StationInformationTopic { get; set; } = string.Empty;
        public string StationStatusTopic { get; set; } = string.Empty;
        public string VehicleTypesTopic { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        //public string StationInformationGroup { get; set; } = string.Empty;
        //public string StationStatusGroup { get; set; } = string.Empty;
        //public string VehicleTypesGroup { get; set; } = string.Empty;
        public string MySqlConnectionString { get; set; } = string.Empty;
        public string MongoConnectionString { get; set; } = string.Empty;
        public string MongoDbName { get; set; } = string.Empty;
        public string MongoCollectionName { get; set; } = string.Empty;
    }
}
