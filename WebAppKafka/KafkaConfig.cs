using Confluent.Kafka;

namespace WebAppKafka.Config
{
    public class KafkaConfig
    {
        public ProducerConfig ProducerConfig { get; private set; }
        public AdminClientConfig AdminConfig { get; private set; }

        public KafkaConfig()
        {
            // Common Kafka Configuration (adjust as needed)
            ProducerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092", // Your Kafka broker address
                ClientId = "my-app",
            };

            AdminConfig = new AdminClientConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }
    }
}
