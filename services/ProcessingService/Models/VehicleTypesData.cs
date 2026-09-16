using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProcessingService.Models
{
    public class VehicleTypesData
    {
        [JsonPropertyName("vehicle_types")]
        public List<VehicleType> VehicleTypes { get; set; } = new();
    }
}
