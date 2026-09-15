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
    public class VehicleTypesService
    {
        private readonly ConfigStrings _configStrings;
        private readonly HttpClient _client;
        private readonly ILogger<StationInformationService> _logger;
        public VehicleTypesService(ConfigStrings strings,
            IHttpClientFactory httpClientFactory,
            ILogger<StationInformationService> logger)
        {
            _configStrings = strings;
            _client = httpClientFactory.CreateClient();
            _logger = logger;
        }
        public async Task<VehicleTypesResponse> GetVehicleTypesAsync()
        {
            try
            {
                VehicleTypesResponse? typesResponse = await _client.GetFromJsonAsync<VehicleTypesResponse>(
                    _configStrings.VehicleTypesUrl,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                return typesResponse ?? new VehicleTypesResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in get vehicle types: {Error}", ex);
            }
            return new VehicleTypesResponse();
        }
    }
}
