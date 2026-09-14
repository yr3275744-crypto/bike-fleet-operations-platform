using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestionService.Models
{
    public class configStrings
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string StationInformationTopic { get; set; } = string.Empty;
        public string StationStatusTopic { get; set; } = string.Empty;
        public string VehicleTypesTopic { get; set; } = string.Empty;
        public string StationInformationUrl { get; set; } = string.Empty;
        public string StationStatusUrl { get; set; } = string.Empty;
        public string VehicleTypesUrl { get; set; } = string.Empty;
    }
}
