using System.Text.Json.Serialization;
namespace Geolocation.Blazor
{
    public class GeolocationPositionError
    {
        [JsonPropertyName("code")]
        public ushort Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return Message; 
        }
    }
}
