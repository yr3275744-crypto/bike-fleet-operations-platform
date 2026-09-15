using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IngestionService.Models
{
    public class VehicleTypeAvailable
    {
        [JsonPropertyName("vehicle_type_id")]
        public string VehicleTypeId { get; set; } = string.Empty;

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
