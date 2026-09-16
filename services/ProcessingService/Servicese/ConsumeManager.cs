using Confluent.Kafka;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using ProcessingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ProcessingService.Servicese
{
    public class ConsumeManager
    {
        private readonly StationStatusHandler _stationStatusHandler;
        private readonly StationInformationHandler _stationInformationHandler;
        private readonly VehicleTypesHandler _vehicleTypesHandler;
        private readonly ConfigStrings _configStrings;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<ConsumeManager> _logger;
        public ConsumeManager(StationInformationHandler stationInformationHandler,
            StationStatusHandler stationStatusHandler,
            VehicleTypesHandler vehicleTypesHandler,
            ConfigStrings configStrings,
            ILogger<ConsumeManager> logger)
        {
            _configStrings = configStrings;
            _stationInformationHandler = stationInformationHandler;
            _stationStatusHandler = stationStatusHandler;
            _vehicleTypesHandler = vehicleTypesHandler;
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = _configStrings.BootstrapServers,
                GroupId = _configStrings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }
        public async Task ConsumeLoop()
        {
            List<string> topics = new();
            topics.Add(_configStrings.StationInformationTopic);
            topics.Add(_configStrings.StationStatusTopic);
            topics.Add(_configStrings.VehicleTypesTopic);
            _consumer.Subscribe(topics);
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating. i take it from confluent docs
                cts.Cancel();
            };
            try
            {
                while (true)
                {
                    var consumeResult = _consumer.Consume(cts.Token);
                    if (consumeResult == null || consumeResult.Message.Value == null)
                    {
                        _logger.LogError("consume faild");
                        continue;
                    }
                    if (consumeResult.Topic == _configStrings.StationInformationTopic)
                    {
                        StationInformation? information = JsonSerializer
                            .Deserialize<StationInformation>(consumeResult.Message.Value);
                        if (information == null)
                        {
                            _logger.LogError("invalid station information message");
                            continue;
                        }
                        bool isCreated = await _stationInformationHandler.CreateAsync(information);
                        if (!isCreated)
                        {
                            await _stationInformationHandler.UpdateAsync(information);
                        }
                        _logger.LogInformation("station information send");
                    }
                    else if (consumeResult.Topic == _configStrings.StationStatusTopic)
                    {
                        StationStatusMongoDto? status = JsonSerializer
                            .Deserialize<StationStatusMongoDto>(consumeResult.Message.Value);
                        if (status == null)
                        {
                            _logger.LogError("invalid station status message");
                            continue;
                        }
                        await _stationStatusHandler.CreateAsync(status);
                        _logger.LogInformation("station status send");
                    }
                    else if (consumeResult.Topic == _configStrings.VehicleTypesTopic)
                    {
                        VehicleType? vehicleType = JsonSerializer
                            .Deserialize<VehicleType>(consumeResult.Message.Value);
                        if (vehicleType == null)
                        {
                            _logger.LogError("invalid vehicle type message");
                            continue;
                        }

                        var isCreated = await _vehicleTypesHandler.CreateAsync(vehicleType);
                        if (!isCreated)
                        {
                            await _vehicleTypesHandler.UpdateAsync(vehicleType);
                        }
                        _logger.LogInformation("vehicle type send");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Ctrl-C was pressed");// Ctrl-C was pressed.
            }
            finally
            {
                _consumer.Close();
            }
        }
    }
}
