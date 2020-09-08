using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Linq;

namespace Geolocation.Blazor
{
    public class GeolocationService : IGeolocationService
    {
        public event EventHandler<PositionChangedEventArgs> PositionChanged;
        public event EventHandler<PositionErrorEventArgs> PositionError;

        private readonly IJSRuntime jsRuntime;
        private readonly Dictionary<long, List<IDisposable>> disposablesByWatchId = new Dictionary<long, List<IDisposable>>();

        public GeolocationService(IJSRuntime jSRuntime)
        {
            jsRuntime = jSRuntime;
        }

        public async Task<GeolocationPosition> GetCurrentPositionAsync(PositionOptions options = null)
        {
            var tcs = new TaskCompletionSource<Tuple<GeolocationPosition, GeolocationPositionError>>();

            await GetCurrentPositionAsync((GeolocationPosition pos) => tcs.SetResult(new Tuple<GeolocationPosition, GeolocationPositionError>(pos, null)),
                                          (GeolocationPositionError err) => tcs.SetResult(new Tuple<GeolocationPosition, GeolocationPositionError>(null, err)),
                                          options);

            var (position, error) = await tcs.Task;
            if (!(position is null) && error is null)
            {
                return position;
            }
            else
            {
                throw new GeolocationPositionException(error.Message) { Code = error.Code };
            }
        }

        public async Task GetCurrentPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null)
        {


            var successJsAction = new JSAction<GeolocationPosition>(success);
            var errorJsAction = new JSAction<GeolocationPositionError>(error);

            var successRef = DotNetObjectReference.Create(successJsAction);
            var errorRef = DotNetObjectReference.Create(errorJsAction);

            await InjectJSHelper();

            await jsRuntime.InvokeVoidAsync("GeolocationBlazor.GetCurrentPosition", successRef, errorRef, options);
        }

        public Task AttachEvents()
        {
            return WatchPositionAsync(pos => OnPositionChanged(new PositionChangedEventArgs { Position = pos }),
                                      err => OnPositionError(new PositionErrorEventArgs { Error = err }));
        }

        public Task ClearWatchAsync(long id)
        {
            disposablesByWatchId.Remove(id, out var disposables);
            disposables.ForEach(disposable => disposable.Dispose());

            return jsRuntime.InvokeVoidAsync("navigator.geolocation.clearWatch", id).AsTask();
        }

        public async Task<long> WatchPositionAsync(Action<GeolocationPosition> success, Action<GeolocationPositionError> error = null, PositionOptions options = null)
        {
            var successJsAction = new JSAction<GeolocationPosition>(success);
            var errorJsAction = new JSAction<GeolocationPositionError>(error);

            var successRef = DotNetObjectReference.Create(successJsAction);
            var errorRef = DotNetObjectReference.Create(errorJsAction);

            await InjectJSHelper();

            long id = await jsRuntime.InvokeAsync<long>("GeolocationBlazor.WatchPosition", successRef, errorRef, options).AsTask();

            disposablesByWatchId.Add(id, new List<IDisposable> { successRef, errorRef } );

            return id;
        }

        protected void OnPositionChanged(PositionChangedEventArgs eventArgs)
        {
            PositionChanged?.Invoke(this, eventArgs);
        }

        protected void OnPositionError(PositionErrorEventArgs eventArgs)
        {
            PositionError?.Invoke(this, eventArgs);
        }

        protected ValueTask InjectJSHelper()
            => jsRuntime.InvokeVoidAsync("eval",// class instance to POJO conversion (ClassToObject) is neccesary, otherwise serialisation does not work
                @"var GeolocationBlazor = {
                    GetCurrentPosition:
                        function(successActionRef, errorActionRef, options) {
                            navigator.geolocation.getCurrentPosition(
                                function(position) {
                                    successActionRef?.invokeMethodAsync('Invoke',{""coords"":GeolocationBlazor.ClassToObject(position.coords), ""timestamp"":position.timestamp});
                                    successActionRef?.dispose();
                                },
                                function(error) {
                                    errorActionRef?.invokeMethodAsync('Invoke', GeolocationBlazor.ClassToObject(error));
                                    errorActionRef?.dispose();
                                },
                                options
                            );
                        },
                    WatchPosition:
                        function(successActionRef, errorActionRef, options) {
                            return navigator.geolocation.watchPosition(
                                function(position) {
                                    successActionRef?.invokeMethodAsync('Invoke',{""coords"":GeolocationBlazor.ClassToObject(position.coords), ""timestamp"":position.timestamp});
                                },
                                function(error) {
                                    errorActionRef?.invokeMethodAsync('Invoke', GeolocationBlazor.ClassToObject(error));
                                },
                                options
                            );
                        },
                    ClassToObject:
                        function(theClass) {
                            var originalClass = theClass || {};
                            var keys = Object.getOwnPropertyNames(Object.getPrototypeOf(originalClass));
                            return keys.reduce((classAsObj, key) => {
                                classAsObj[key] = originalClass[key];
                                return classAsObj;
                            }, {});
                        }
                    }");

    }
}