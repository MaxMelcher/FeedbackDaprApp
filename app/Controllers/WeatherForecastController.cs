using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;

namespace app.Controllers
{

    public class Widget
    {
        public string Size { get; set; }
        public string Color { get; set; }
    }

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

            await client.SaveStateAsync(storeName, stateKeyName, feedback);
            var state = new Widget() { Size = "small", Color = "yellow", };
            await client.SaveStateAsync(storeName, stateKeyName, state);
            Console.WriteLine("Saved State!"); // WE ARE DAPR!
        }

        [HttpGet("GetFeedback")]
        public async Task<HttpResponseMessage> Get(int sessionId)
        {
            var client = new DaprClientBuilder()
            .Build();

            var http = new HttpClient();
            //curl -s -X POST -H "Content-Type: application/json" -d @query-api-examples/query1.json http://localhost:3500/v1.0-alpha1/state/statestore/query

            var storeName = "statestore";
            var query = "{'query': { 'filter': {'EQ': { 'value.SessionId': '1' }}}}";
            //make a get query and format the result as json
            var data = await http.PostAsync($"http://localhost:3500/v1.0-alpha1/state/{storeName}/query", new StringContent(query, Encoding.UTF8, "application/json"));

            return data;
        }
    }

}
