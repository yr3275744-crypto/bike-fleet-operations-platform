using Confluent.Kafka;
using IngestionService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IngestionService.services
{
    public class Orchestrator
    {
        //private readonly ServiceProvider _serviceProvider;
        private readonly ILogger<Orchestrator> _logger;
        private readonly StationInformationService _stationInformationService;
        private readonly StationStatusService _stationStatusService;
        private readonly VehicleTypesService _vehicleTypesService;
        private readonly ProducerService _producerService;
        public Orchestrator(StationInformationService stationInformationService,
            StationStatusService stationStatusService,
            VehicleTypesService vehicleTypesService,
            ProducerService producerService,
            ILogger<Orchestrator> logger)
        {
            //_serviceProvider = serviceProvider;
            _logger = logger;
            _stationInformationService = stationInformationService;
            _stationStatusService = stationStatusService;
            _vehicleTypesService = vehicleTypesService;
            _producerService = producerService;
        }
        public async Task Play()
        {
            //StationInformationService stationInformationService = _serviceProvider.GetRequiredService<StationInformationService>();
            //StationStatusService stationStatusService = _serviceProvider.GetRequiredService<StationStatusService>();
            //VehicleTypesService vehicleTypesService = _serviceProvider.GetRequiredService<VehicleTypesService>();
            //ProducerService producerService = _serviceProvider.GetRequiredService<ProducerService>();
            //ConfigStrings strings = _serviceProvider.GetRequiredService<ConfigStrings>();
            

            StationInformationResponse informationResponse = await _stationInformationService.GetStationInformationAsync();
            StationStatusResponse statusResponse = await _stationStatusService.GetStationStatusAsync();
            VehicleTypesResponse vehicleTypesReponse = await _vehicleTypesService.GetVehicleTypesAsync();

            await _producerService.SendInformations();
            await _producerService.SendStatuses();
            await _producerService.SendVehicleTypes();

            _producerService.Producer.Flush();
            _producerService.Producer.Dispose();
        }
    }
}
