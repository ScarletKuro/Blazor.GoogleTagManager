using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Blazor.GoogleTagManager
{
    /// <summary>
    /// Implementation for <see cref="IGoogleTagManager"/>.
    /// </summary>
    public class GoogleTagManager : IGoogleTagManager, IAsyncDisposable
    {
        private readonly GoogleTagManagerOptions _gtmOptions;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        private IJSObjectReference? _jsModule;

        /// <inheritdoc/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc/>
        public bool IsTackingEnabled { get; private set; } = true;

        public GoogleTagManager(
            IOptions<GoogleTagManagerOptions> gtmOptions,
            NavigationManager navigationManager,
            IJSRuntime jsRuntime)
        {
            _gtmOptions = gtmOptions.Value;
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
        }

        /// <inheritdoc/>
        public void EnableTracking()
        {
            IsTackingEnabled = true;
        }

        /// <inheritdoc/>
        public void DisableTracking()
        {
            IsTackingEnabled = false;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return;
            }

            if (string.IsNullOrEmpty(_gtmOptions.GtmId))
            {
                throw new ArgumentException("GTM Id cannot be empty.", nameof(_gtmOptions.GtmId));
            }

            _jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.GoogleTagManager/GoogleTagManager.js");

            await _jsModule.InvokeVoidAsync("initialize", _gtmOptions.GtmId, _gtmOptions.Attributes, _gtmOptions.DebugToConsole);

            IsInitialized = true;
        }

        /// <inheritdoc/>
        public async Task PushAsync(object data)
        {
            if (!IsTackingEnabled)
            {
                return;
            }

            await InitializeAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("push", data, _gtmOptions.DebugToConsole);
            }
        }

        /// <inheritdoc/>
        public async Task PushEventAsync(string eventName, object? eventData = null)
        {
            if (!IsTackingEnabled)
            {
                return;
            }

            await InitializeAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("pushEvent", eventName, eventData, _gtmOptions.DebugToConsole);
            }
        }

        /// <inheritdoc/>
        public async Task PushPageViewAsync(object? additionalData = null)
        {
            await PushPageViewCoreAsync(_navigationManager.Uri, additionalData);
        }

        /// <inheritdoc/>
        async Task IGoogleTagManager.PushPageViewAsync(LocationChangedEventArgs? args)
        {
            if (!IsTackingEnabled)
            {
                return;
            }

            if (args is null)
            {
                // App firstRender
                await PushPageViewAsync();
            }
            else
            {
                await PushPageViewCoreAsync(args.Location, new Dictionary<string, string> { { "isNavigationIntercepted", args.IsNavigationIntercepted.ToString() } });
            }
        }

        private async Task PushPageViewCoreAsync(string url, object? additionalData = null)
        {
            if (!IsTackingEnabled)
            {
                return;
            }

            await InitializeAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("pushPageViewEvent", _gtmOptions.PageViewEventName, _gtmOptions.PageViewUrlVariableName, url, additionalData, _gtmOptions.DebugToConsole);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
    }
}