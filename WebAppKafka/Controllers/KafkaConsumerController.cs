using Microsoft.AspNetCore.Mvc;
using WebAppKafka.Services;

namespace WebAppKafka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaConsumerController : ControllerBase
    {
        private readonly KafkaConsumerService _kafkaConsumerService;
        private CancellationTokenSource _cts;

        public KafkaConsumerController(KafkaConsumerService kafkaConsumerService)
        {
            _kafkaConsumerService = kafkaConsumerService;
            _cts = new CancellationTokenSource();
        }

        [HttpPost("start-consumer")]
        public async Task<IActionResult> StartConsumer()
        {
            // Start consuming messages
            var result = await _kafkaConsumerService.ConsumeMessages(_cts.Token);
            return Ok(result);
        }

        [HttpPost("stop-consumer")]
        public IActionResult StopConsumer()
        {
            // Stop consuming messages
            _cts.Cancel();
            return Ok("Kafka consumer stopped.");
        }
    }
}
