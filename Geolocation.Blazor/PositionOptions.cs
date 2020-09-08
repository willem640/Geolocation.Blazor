using System.Text.Json.Serialization;
namespace Geolocation.Blazor
{
    public class PositionOptions
    {
        [JsonPropertyName("enableHighAccuracy")]
        public bool EnableHighAccuracy { get; set; } = false;

        [JsonPropertyName("timeout")]
        public long Timeout { get; set; } = long.MaxValue;

        [JsonPropertyName("maximumAge")]
        public long MaximumAge { get; set; } = 0;

        public override string ToString()
        {
            return $"EnableHighAccuracy: {EnableHighAccuracy}, Timeout: {Timeout}, MaximumAge: {MaximumAge}";
        }
    }
}
