using Microsoft.AspNetCore.Mvc;
using WebAppKafka.Infrastructure;
using WebAppKafka.Services;

namespace WebAppKafka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaProducerController : ControllerBase
    {
        private readonly KafkaProducerService _kafkaProducerService;
        private readonly KafkaDBContext _kafkaDBContext;
        public KafkaProducerController(KafkaProducerService kafkaProducerService, KafkaDBContext kafkaDBContext)
        {
            _kafkaProducerService = kafkaProducerService;
            _kafkaDBContext = kafkaDBContext;
        }

        [HttpPost("produce-message")]
        public async Task<IActionResult> ProduceMessage()
        {
            var user = _kafkaDBContext.Riders.ToList();
            await _kafkaProducerService.ProduceMessageAsync(user);
            return Ok();
        }
    }
}
