using System.Text.Json.Serialization;

namespace app.Models
{
    public class EqObject
    {
        [JsonPropertyName("value.sessionId")]
        public string SessionId { get; set; }
    }

}
