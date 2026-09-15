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
    public class StationStatusService
    {
        private readonly ConfigStrings _configStrings;
        private readonly HttpClient _client;
        private readonly ILogger<StationInformationService> _logger;
        public StationStatusService(ConfigStrings strings,
            IHttpClientFactory httpClientFactory,
            ILogger<StationInformationService> logger)
        {
            _configStrings = strings;
            _client = httpClientFactory.CreateClient();
            _logger = logger;
        }
        public async Task<StationStatusResponse> GetStationStatusAsync()
        {
            try
            {
                StationStatusResponse? statusResponse = await _client.GetFromJsonAsync<StationStatusResponse>(
                    _configStrings.StationStatusUrl,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                return statusResponse ?? new StationStatusResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in get station status: {Error}", ex);
            }
            return new StationStatusResponse();
        }
        public bool ValidateStatus(StationStatus status)
        {
            if (status.NumBikesAvailable < 0 ||
                status.NumBikesDisabled < 0 ||
                status.NumDocksAvailable < 0 ||
                status.NumDocksDisabled < 0
                )
            {
                _logger.LogInformation("invalid station status: negative num is not valid");
                return false;
            }
            return true;
        }
    }
}
