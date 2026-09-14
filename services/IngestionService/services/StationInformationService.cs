using IngestionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestionService.services
{
    public class StationInformationService
    {
        private readonly configStrings _configStrings;
        private readonly IHttpClientFactory _httpClientFactory;
        public StationInformationService(configStrings configStrings,
            IHttpClientFactory httpClientFactory)
        {
            _configStrings = configStrings;
            _httpClientFactory = httpClientFactory;
        }
    }
}
