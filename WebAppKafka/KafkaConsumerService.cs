using Confluent.Kafka;
using WebAppKafka.Infrastructure;
using WebAppKafka.Models;

namespace WebAppKafka.Services
{
    public class KafkaConsumerService
    {
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly KafkaDBContext _kafkaDBContext;
        public KafkaConsumerService(ILogger<KafkaConsumerService> logger, KafkaDBContext kafkaDBContext)
        {
            _logger = logger;
            _kafkaDBContext = kafkaDBContext;
        }

        public async Task<List<RiderLocationLog>> ConsumeMessages(CancellationToken stoppingToken)
        {
            var listRiderLocationLog = new List<RiderLocationLog>();

            string group = "group-1";
            var consumeConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = group,
            };
            using (var consumer = new ConsumerBuilder<Ignore, string>(consumeConfig).Build())
            {
                _logger.LogInformation("Consumer Subscribe Topic.");
                consumer.Subscribe(KafkaConstants.topicName);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("Consumer Consume from the Topic.");
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult != null && !string.IsNullOrEmpty(consumeResult.Message.Value))
                        {
                            var riders = System.Text.Json.JsonSerializer.Deserialize<List<RiderWithGroup>>(consumeResult.Message.Value);

                            if (riders.Any())
                            {
                                _logger.LogInformation("data from the Consumer Consume");
                                await ProcessRidersLogsAsync(riders, group, listRiderLocationLog);
                            }
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error occurred: {e.Error.Reason}");
                }
                finally
                {
                    consumer.Close();
                    _logger.LogInformation("Consumer disconnected.");
                }
            }

            return listRiderLocationLog; // Return the list of consumed messages
        }
        private async Task ProcessRidersLogsAsync(List<RiderWithGroup> riders, string groupId, List<RiderLocationLog> listRiderLocationLog)
        {
            foreach (var rider in riders)
            {
                rider.GroupId = groupId + "App3";
                var riderLocationLog = rider.getRiderLocationLogs();
                listRiderLocationLog.Add(riderLocationLog);
            }
            await _kafkaDBContext.AddRangeAsync(listRiderLocationLog);
            await _kafkaDBContext.SaveChangesAsync();
        }
        private async Task ProcessMessageAsync(ConsumeResult<Ignore, string> consumeResult)
        {
            await Task.Delay(100); // Simulating asynchronous message processing
            _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' from topic {consumeResult.Topic}");
        }
    }
}
