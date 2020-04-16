using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Models;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Repository;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private IUnitOfWork _unitOfWork { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("AddWeatherForecast")]
        public IActionResult AddWeatherForecast(WeatherForecastViewModel model)
        {
            // Per: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1
            // Web API controllers don't have to check ModelState.IsValid if they have the [ApiController] attribute.
            // Still gonna leave it here for good old times
            if (!ModelState.IsValid)
                return BadRequest();

            // Validation is automatic, but you might want to repeat it manually. For example, you might compute a value
            // for a property and want to rerun validation after setting the property to the computed value
            if (!TryValidateModel(model, nameof(WeatherForecastViewModel)))
            {
                return BadRequest();
            }

            try
            {
                var wForecast = new WeatherForecast
                {
                    PlaceId = model.PlaceId,
                    TemperatureC = model.TemperatureC,
                    Date = Convert.ToDateTime(model.Date),
                    Summary = model.Summary
                };

                _unitOfWork.Repo<WeatherForecast>().Add(wForecast);
                _unitOfWork.Commit();

                return Ok(wForecast); // wForecast object will have Id after it has been inserted into database here
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("GetForecasts")]
        public IActionResult GetForecasts()
        {
            try
            {
                var forecasts = _unitOfWork.Repo<WeatherForecast>().GetAll();
                var retList = forecasts.Select(weatherForecast => new WeatherForecastViewModel
                {
                    Id = weatherForecast.Id,
                    Date = weatherForecast.Date.ToString(CultureInfo.InvariantCulture),
                    PlaceId = weatherForecast.PlaceId,
                    Summary = weatherForecast.Summary,
                    TemperatureC = weatherForecast.TemperatureC
                }).ToList();

                return Ok(retList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("GetForecast/{id}")]
        public IActionResult GetForecast(long id)
        {
            try
            {
                var forecast = _unitOfWork.Repo<WeatherForecast>().Query(s => s.Id == id).FirstOrDefault();
                if (forecast == null)
                    return NotFound();

                var retForecast = new WeatherForecastViewModel
                {
                    Id = forecast.Id,
                    Date = forecast.Date.ToString(CultureInfo.InvariantCulture),
                    PlaceId = forecast.PlaceId,
                    Summary = forecast.Summary,
                    TemperatureC = forecast.TemperatureC
                };

                return Ok(retForecast);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
