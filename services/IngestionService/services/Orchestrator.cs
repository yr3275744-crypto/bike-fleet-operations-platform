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
            try
            {
                var stationStatusTimer = new PeriodicTimer(TimeSpan.FromMinutes(60));
                var stationInformationTimer = new PeriodicTimer(TimeSpan.FromMinutes(60));
                var vehicleTypesTimer = new PeriodicTimer(TimeSpan.FromSeconds(60));

                await Task.WhenAll(_producerService.SendInformations(),
                    _producerService.SendStatuses(),
                    _producerService.SendVehicleTypes());
                //await _producerService.SendInformations();
                //await _producerService.SendStatuses();
                //await _producerService.SendVehicleTypes();

                Task<bool> statusTask = stationStatusTimer.WaitForNextTickAsync().AsTask();
                Task<bool> informationTask = stationInformationTimer.WaitForNextTickAsync().AsTask();
                Task<bool> vehicleTypesTask = vehicleTypesTimer.WaitForNextTickAsync().AsTask();

                while (true)
                {
                    Task<bool> completedTask = await Task.WhenAny(
                        statusTask,
                        informationTask,
                        vehicleTypesTask);
                    if (completedTask == statusTask)
                    {
                        await _producerService.SendStatuses();
                        statusTask = stationStatusTimer.WaitForNextTickAsync().AsTask();
                    }
                    if (completedTask == informationTask)
                    {
                        await _producerService.SendInformations();

                        informationTask = stationInformationTimer.WaitForNextTickAsync().AsTask();
                    }

                    if (completedTask == vehicleTypesTask)
                    {
                        await _producerService.SendVehicleTypes();

                        vehicleTypesTask = vehicleTypesTimer.WaitForNextTickAsync().AsTask();
                    }
                }
            }
            finally
            {
                _producerService.Producer.Flush();
                _producerService.Producer.Dispose();
            }
        }
    }
}
