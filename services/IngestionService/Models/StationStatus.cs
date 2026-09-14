using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IngestionService.Models
{
    public class StationStatus
    {
        [JsonPropertyName("station_id")]
        public string StationId { get; set; } = string.Empty;

        [JsonPropertyName("is_installed")]
        public bool IsInstalled { get; set; }

        [JsonPropertyName("is_renting")]
        public bool IsRenting { get; set; }

        [JsonPropertyName("is_returning")]
        public bool IsReturning { get; set; }

        [JsonPropertyName("last_reported")]
        public long LastReported { get; set; }

        [JsonPropertyName("num_docks_available")]
        public int NumDocksAvailable { get; set; }

        [JsonPropertyName("num_bikes_available")]
        public int NumBikesAvailable { get; set; }

        [JsonPropertyName("num_docks_disabled")]
        public int NumDocksDisabled { get; set; }

        [JsonPropertyName("num_bikes_disabled")]
        public int NumBikesDisabled { get; set; }
    }
}
