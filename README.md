# Geolocation.Blazor
A small Blazor library for interacting with the JavaScript Geolocation API.

It's made to be (nearly) exactly like the navigator.geolocation API, and it additionally offers C# events (PositionChanged, PositionError) and an extra method (GetCurrentPositionAsync(PositionOptions)) that gets the location without having to provide a delegate.


Not (yet) on NuGet, but you can easily add it as a project reference, or just compile it and reference the dlls.
