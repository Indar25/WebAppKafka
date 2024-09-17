using Confluent.Kafka;
using Confluent.Kafka.Admin;
using WebAppKafka.Config;

namespace WebAppKafka.Services
{
    public class KafkaAdminService
    {
        private readonly AdminClientConfig _adminConfig;
        private readonly ILogger<KafkaAdminService> _logger;

        public KafkaAdminService(KafkaConfig kafkaConfig, ILogger<KafkaAdminService> logger)
        {
            _adminConfig = kafkaConfig.AdminConfig;
            _logger = logger;
        }

        public async Task CreateTopicAsync(string topicName, int numPartitions = 1)
        {
            using (var adminClient = new AdminClientBuilder(_adminConfig).Build())
            {
                try
                {
                    var topics = new List<TopicSpecification>
                    {
                        new TopicSpecification { Name = topicName, NumPartitions = numPartitions,ReplicationFactor = 1 }
                    };

                    await adminClient.CreateTopicsAsync(topics);
                    _logger.LogInformation($"Topic {topicName} created successfully.");
                }
                catch (CreateTopicsException e)
                {
                    _logger.LogError($"Error creating topic: {e.Results[0].Error.Reason}");
                }
            }
        }
    }
}
