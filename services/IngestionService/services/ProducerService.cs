using Confluent.Kafka;
using IngestionService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IngestionService.services
{
    public class ProducerService
    {
        private readonly ConfigStrings _configStrings;
        public IProducer<Null, string> Producer { get; private set; }
        private readonly StationInformationService _informationService;
        private readonly StationStatusService _stationStatusService;
        private readonly VehicleTypesService _vehicleTypesService;
        private readonly ILogger<ProducerService> _logger;
        public ProducerService(ConfigStrings strings,
            ILogger<ProducerService> logger,
            StationInformationService informationService,
            StationStatusService stationStatusService,
            VehicleTypesService vehicleTypesService)
        {
            _configStrings = strings;
            _logger = logger;
            _informationService = informationService;
            _stationStatusService = stationStatusService;
            _vehicleTypesService = vehicleTypesService;
            var config = new ProducerConfig
            {
                BootstrapServers = strings.BootstrapServers
            };
            Producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task SendInformations()
        {
            int messagSend = 0;
            StationInformationResponse informationResponse = await 
                _informationService.GetStationInformationAsync();
            foreach (StationInformation information in informationResponse.Data.Stations)
            {
                bool isValid = _informationService.ValidateInformation(information);
                if (isValid)
                {
                    string value = JsonSerializer.Serialize(information);
                    var message = new Message<Null, string> { Value = value };
                    var respone = await Producer.ProduceAsync(_configStrings.StationInformationTopic, message);
                    if (respone == null || respone.Status != PersistenceStatus.Persisted)
                    {
                        _logger.LogError($"the information send faild.");
                        continue;
                    }
                    messagSend++;
                    _logger.LogInformation("information send successfully");
                }
            }
            _logger.LogInformation($"information send: {messagSend}");
        }
        public async Task SendStatuses()
        {
            int messagSend = 0;
            StationStatusResponse statusResponse = await
                _stationStatusService.GetStationStatusAsync();
            foreach (StationStatus status in statusResponse.Data.Stations)
            {
                bool isValid = _stationStatusService.ValidateStatus(status);
                if (isValid)
                {
                    string value = JsonSerializer.Serialize(status);
                    var message = new Message<Null, string> { Value = value };
                    var respone = await Producer.ProduceAsync(_configStrings.StationStatusTopic, message);
                    if (respone == null || respone.Status != PersistenceStatus.Persisted)
                    {
                        _logger.LogError($"the status send faild.");
                        continue;
                    }
                    messagSend++;
                    _logger.LogInformation("status send successfully");
                }
            }
            _logger.LogInformation($"statuses send: {messagSend}");
        }
        public async Task SendVehicleTypes()
        {
            int messagSend = 0;
            VehicleTypesResponse typesResponse = await
                _vehicleTypesService.GetVehicleTypesAsync();
            foreach (VehicleType vehicleType in typesResponse.Data.VehicleTypes)
            {
                string value = JsonSerializer.Serialize(vehicleType);
                var message = new Message<Null, string> { Value = value };
                var respone = await Producer.ProduceAsync(_configStrings.VehicleTypesTopic, message);
                if (respone == null || respone.Status != PersistenceStatus.Persisted)
                {
                    _logger.LogError($"the vehicle type send faild.");
                    continue;
                }
                messagSend++;
                _logger.LogInformation("vehicle type send successfully");

            }
            _logger.LogInformation($"vehicle types send: {messagSend}");
        }
    }
}
