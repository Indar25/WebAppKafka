using Microsoft.AspNetCore.Mvc;
using WebAppKafka.Services;

namespace WebAppKafka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaAdminController : ControllerBase
    {
        private readonly KafkaAdminService _kafkaService;

        public KafkaAdminController(KafkaAdminService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        [HttpPost("create-topic")]
        public async Task<IActionResult> CreateTopic([FromQuery] string topicName, [FromQuery] int partitions = 1)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                return BadRequest("Topic name cannot be null or empty.");
            }

            await _kafkaService.CreateTopicAsync(topicName, partitions);
            return Ok($"Topic {topicName} created with {partitions} partitions.");
        }
    }
}
