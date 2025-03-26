
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult LogError([FromBody] string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest("Error message cannot be empty.");
            }

            _logger.LogError("Error occurred: {ErrorMessage}", errorMessage);
            return Ok("Error logged successfully.");
        }
    }
}
