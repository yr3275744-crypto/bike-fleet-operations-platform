using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IngestionService.Models
{
    public class StationStatusData
    {
        [JsonPropertyName("stations")]
        public List<StationStatus> Stations { get; set; } = new();
    }
}
