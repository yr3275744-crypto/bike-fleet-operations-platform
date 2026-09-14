using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IngestionService.Models
{
    public class StationInformationData
    {
        [JsonPropertyName("stations")]
        public List<StationInformation> Stations { get; set; } = new();
    }
}
