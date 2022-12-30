using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AppSettingsDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public WeatherForecastController(IConfiguration _configuration)
        {
            this._configuration= _configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //var result = _appconfig.GetTestValue();
            var result = _configuration["TestKey"];
            var upTest = _configuration["test"];
            return Ok(result);
        }
    }
}