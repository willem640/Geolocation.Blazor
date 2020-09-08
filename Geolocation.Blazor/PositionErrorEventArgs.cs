using System;
namespace Geolocation.Blazor
{
    public class PositionErrorEventArgs : EventArgs
    {
        public GeolocationPositionError Error { get; set; }
    }
}
