﻿using System;
using Microsoft.JSInterop;

namespace Geolocation.Blazor
{
    internal class JSAction<T>
    {
        public Action<T> Action { get; set; }

        public JSAction()
        {

        }

        public JSAction(Action<T> action)
        {
            Action = action;
        }

        [JSInvokable]
        public void Invoke(T arg)
        {
            Action?.Invoke(arg);
        }

        public static implicit operator JSAction<T>(Action<T> action)
        {
            return new JSAction<T>(new Action<T>(action));
        }
    }


}
