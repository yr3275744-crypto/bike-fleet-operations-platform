using System.Text.Json.Serialization;

namespace API.Models
{
    public class GetStationsResponseDto
    {
        public int NumDocksAvailable { get; set; }
        public int NumBikesAvailable { get; set; }
        public int IsRenting { get; set; }
        public int IsReturning { get; set; }
        public int Capacity { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string StationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
