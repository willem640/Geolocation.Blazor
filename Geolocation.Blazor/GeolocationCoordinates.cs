using System.Text.Json.Serialization;

namespace Geolocation.Blazor
{
    public class GeolocationCoordinates
    { // all these properties look awful, but I can't pass any options to the deserialiser in the js -> .net interop (and I really like my PascalCasing)
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("altitude")]
        public double? Altitude { get; set; }

        [JsonPropertyName("accuracy")]
        public double Accuracy { get; set; }

        [JsonPropertyName("altitudeAccuracy")]
        public double? AltitudeAccuracy { get; set; }

        [JsonPropertyName("heading")]
        public double? Heading { get; set; }

        [JsonPropertyName("speed")]
        public double? Speed { get; set; }

        public override string ToString()
        {
            return $"{Latitude:f} {Longitude:f}";
        }
    }
}
