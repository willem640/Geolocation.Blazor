using System;
namespace Geolocation.Blazor
{
    /// <summary>
    /// A class that wraps a <see cref="GeolocationPosition"/> for use in events
    /// </summary>
    public class PositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="GeolocationPosition"/> that is wrapped
        /// </summary>
        public GeolocationPosition Position { get; set; }
    }
}
