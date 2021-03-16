using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceClientA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private IConsulClient _consulClient;
        private IConfiguration _configuration;
        private ILogger<ServiceController> _logger;
        public ServiceController(IConsulClient consulClient, IConfiguration configuration, ILogger<ServiceController> logger)
        {
            _consulClient = consulClient;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/<ServiceController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _consulClient.Agent.Services().Result.Response;
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("/{key}")]
        public IActionResult GetConfig(string key)
        {
            var result = _consulClient.KV.Get(key).Result.Response;
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Config")]
        public IActionResult GetConfigJson()
        {
            var result = _configuration["Config"];
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Notification")]
        public IActionResult Notification()
        {
            var bytes = new byte[1024];
            Request.Body.ReadAsync(bytes, 0, bytes.Length);
            string content = Encoding.UTF8.GetString(bytes).Trim('\0');
            _logger.LogWarning(JsonSerializer.Serialize(content));
            return Ok();
        }
    }
}
