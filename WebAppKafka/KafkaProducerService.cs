using Confluent.Kafka;
using WebAppKafka.Config;
using WebAppKafka.Models;

namespace WebAppKafka.Services
{
    public class KafkaProducerService
    {
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(KafkaConfig kafkaConfig, ILogger<KafkaProducerService> logger)
        {
            _producerConfig = kafkaConfig.ProducerConfig;
            _logger = logger;
        }

        public async Task ProduceMessageAsync(List<Rider> riders)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(riders);
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(KafkaConstants.topicName, new Message<Null, string> { Value = message });
                    _logger.LogInformation($"Message delivered to: {result.TopicPartitionOffset}");
                }
                catch (ProduceException<Null, string> e)
                {
                    _logger.LogError($"Error producing message: {e.Error.Reason}");
                }
            }
        }
    }
}
