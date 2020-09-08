using System;
using System.Text.Json.Serialization;

namespace Geolocation.Blazor
{
    public class GeolocationPosition
    {
        [JsonPropertyName("coords")]
        public GeolocationCoordinates Coords { get; set; }

        [JsonPropertyName("timestamp")]
        public ulong Timestamp { get; set; }

        public override string ToString()
        {
            return Coords.ToString();
        }
    }
}
