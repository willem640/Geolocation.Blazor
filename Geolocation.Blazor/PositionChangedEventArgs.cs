using System;
namespace Geolocation.Blazor
{
    public class PositionChangedEventArgs : EventArgs
    {
        public GeolocationPosition Position { get; set; }
    }
}
