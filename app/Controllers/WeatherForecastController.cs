using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var client = new DaprClientBuilder()
            .Build();

            var storeName = "statestore";
            var stateKeyName = "stateKeyName";
            var weathers = await client.GetStateAsync<WeatherForecast[]>(storeName, stateKeyName);

            if (weathers == null || weathers.Length == 0)
            {
                var rng = new Random();
                weathers = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
               .ToArray();

                await client.SaveStateAsync(storeName, stateKeyName, weathers);
                Console.WriteLine("Saved State!");
            }
            return weathers;
        }
    }
}
