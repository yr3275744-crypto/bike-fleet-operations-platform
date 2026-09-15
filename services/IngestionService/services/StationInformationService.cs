using Confluent.Kafka;
using IngestionService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IngestionService.services
{
    public class StationInformationService
    {
        private readonly ConfigStrings _configStrings;
        private readonly HttpClient _client;
        private readonly ILogger<StationInformationService> _logger;
        //private readonly IProducer<Null, string> _producer;
        public StationInformationService(ConfigStrings strings,
            IHttpClientFactory httpClientFactory,
            ILogger<StationInformationService> logger
            )
            //IProducer<Null, string> producer)
        {
            _configStrings = strings;
            _client = httpClientFactory.CreateClient();
            _logger = logger;
            //_producer = producer;
        }
        public async Task<StationInformationResponse> GetStationInformationAsync()
        {
            try
            {
                StationInformationResponse? informationResponse = await _client.GetFromJsonAsync<StationInformationResponse>(
                    _configStrings.StationInformationUrl,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                return informationResponse ?? new StationInformationResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in get station information: {Error}", ex);
            }
            return new StationInformationResponse();
        }
        public bool ValidateInformation(StationInformation information)
        {
            bool isValid = true;
            if (information.StationId == null)
            {
                _logger.LogInformation("invalid station information: stationId must be full");
                isValid = false;
            }
            if (
                information.Latitude < -90 ||
                information.Latitude > 90 ||
                information.Longitude < -180 ||
                information.Longitude > 180
                )
            {
                _logger.LogInformation("invalid station information: latitude or longitude are invalid");
                isValid = false;
            }
            if (information.Capacity < 0)
            {
                _logger.LogInformation("invalid station information: capacity must be > 0");
                isValid = false;
            }
            return isValid;
        }

    }
}
