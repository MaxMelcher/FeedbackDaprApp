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
    [Route("api/weatherforecast")]
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

        [HttpPost("SubmitFeedback")]
        public async Task SubmitFeedback([FromBody] Feedback feedback)
        {
            var client = new DaprClientBuilder()
                        .Build();

            var storeName = "statestore";
            var stateKeyName = Guid.NewGuid().ToString();
            var sessionName = $"session-{feedback.SessionId}";

            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("partitionKey", sessionName);

            await client.SaveStateAsync(storeName, stateKeyName, feedback, null, metadata);

            var sessions = await client.GetStateAsync<Feedback[]>(storeName, null, null, metadata);

            Console.WriteLine("Saved State!"); // WE ARE AWESOME
        }

        /*
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
                        Console.WriteLine("Saved State!"); // WE ARE AWESOME
                    }
                    return weathers;
                }
                */
    }

}
