using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProcessingService.Models
{
    public class VehicleTypesResponse
    {
        [JsonPropertyName("last_updated")]
        public long LastUpdated { get; set; }

        [JsonPropertyName("ttl")]
        public int Ttl { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public VehicleTypesData Data { get; set; } = new();
    }
}
