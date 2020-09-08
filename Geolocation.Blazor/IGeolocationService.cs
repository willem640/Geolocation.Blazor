using System;
using System.Threading.Tasks;

namespace Geolocation.Blazor
{
    public interface IGeolocationService
    {
        Task<GeolocationPosition> GetCurrentPositionAsync(PositionOptions options = null);
        Task GetCurrentPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null);
        Task<long> WatchPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null);
        Task ClearWatchAsync(long id);

        event EventHandler<PositionChangedEventArgs> PositionChanged;
        event EventHandler<PositionErrorEventArgs> PositionError;

    }
}
