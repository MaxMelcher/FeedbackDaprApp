using System;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace app.Controllers
{

    public class Widget
    {
        public string Size { get; set; }
        public string Color { get; set; }
    }

    public class EqObject
    {
        [JsonPropertyName("value.sessionId")]
        public string SessionId { get; set; }
    }

    [ApiController]
    [Route("api/weatherforecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("SubmitFeedback")]
        public async Task SubmitFeedback([FromBody] Feedback feedback)
        {
            var wrapper = new[]
            {
                new {
                    key = Guid.NewGuid().ToString(),
                    value = feedback
                }
            };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(wrapper, options);

            var storeName = "statestore";

            var http = new HttpClient();
            var data = await http.PostAsync($"http://localhost:3500/v1.0/state/{storeName}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!data.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to save state {data.ReasonPhrase}");
            }

            Console.WriteLine("Saved State!");
        }

        [HttpGet("GetFeedback")]
        public async Task<string> Get(string sessionId)
        {
            var http = new HttpClient();
            //curl -s -X POST -H "Content-Type: application/json" -d @query-api-examples/query1.json http://localhost:3500/v1.0-alpha1/state/statestore/query

            var storeName = "statestore";
            var query = new
            {
                query = new
                {
                    filter = new
                    {
                        EQ = new EqObject { SessionId = sessionId }
                    }
                }
            };

            var json = JsonSerializer.Serialize(query);

            //make a get query and format the result as json
            var data = await http.PostAsync($"http://localhost:3500/v1.0-alpha1/state/{storeName}/query",
             new StringContent(json, Encoding.UTF8, "application/json"));

            return await data.Content.ReadAsStringAsync();
        }
    }

}
