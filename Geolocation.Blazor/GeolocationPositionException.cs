using System;
using System.Runtime.Serialization;

namespace Geolocation.Blazor
{
    public class GeolocationPositionException : Exception
    {
        public GeolocationPositionException()
        {
        }

        public GeolocationPositionException(string message) : base(message)
        {
        }

        public GeolocationPositionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GeolocationPositionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ushort Code { get; set; }

    }
}
