# Geolocation.Blazor
A small Blazor library for interacting with the JavaScript Geolocation API.

It's made to be (nearly) exactly like the navigator.geolocation API, and additionally offer C# events (PositionChanged, PositionError), and an extra method (GetCurrentPositionAsync(PositionOptions)) that gets the location without having to provide a delegate.

Notes:
* GeolocationService MUST be Transient

Not (yet) on NuGet, but you can easily add it as a project reference, or just compile it and reference the dlls.

Example:

Index.razor.cs:
```csharp
using System;
using System.Threading.Tasks;
using Geolocation.Blazor;
using Microsoft.AspNetCore.Components;

namespace Example.Pages
{
    public partial class Index
    {
        [Inject]
        private IGeolocationService Geolocation { get; }

        public async Task DoLocationTests()
        {
            try
            {
                GeolocationPosition location = await Geolocation.GetCurrentPositionAsync(new PositionOptions { EnableHighAccuracy = true });

                GeolocationCoordinates coordinates = location.Coords;

                Console.WriteLine($"Latitude: {coordinates.Latitude}\nLongitude: {coordinates.Longitude}\nAltitude: {coordinates.Altitude}");
            }
            catch(GeolocationPositionException e)
            {
                Console.WriteLine($"Error retrieving location: {e.Message}");
            }



            long id = await Geolocation.WatchPositionAsync(pos => Console.WriteLine(pos),
                                                           err => Console.WriteLine(err),
                                                           new PositionOptions { Timeout = 1000 });

            await Task.Delay(5000);

            await Geolocation.ClearWatchAsync(id);

            await Geolocation.AttachEvents();

            Geolocation.PositionChanged += (object sender, PositionChangedEventArgs arg) =>
            {
                Console.WriteLine($"Position Changed: {arg.Position}");
            };
        }
    }
}
```

Program.cs:
```csharp
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Geolocation.Blazor;

namespace Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<IGeolocationService, GeolocationService>();

            await builder.Build().RunAsync();
        }
    }
}
```
