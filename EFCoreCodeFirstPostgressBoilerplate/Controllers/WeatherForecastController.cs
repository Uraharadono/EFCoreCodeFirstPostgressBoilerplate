using System;
using System.Linq;
using EFCoreCodeFirstPostgressBoilerplate.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EFCoreCodeFirstPostgressBoilerplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MyDbContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _context.WeatherForecasts.ToList();
                if (!data.Any())
                    return NotFound();

                // Newtonsoft.Json.JsonConvert.SerializeObject()
                return Ok(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
    }
}
