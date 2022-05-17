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

		private bool _isInitialized;
		private IJSObjectReference? _jsModule;

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
		public async Task InitializeAsync()
		{
			if (_isInitialized)
			{
				return;
			}

            _jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.GoogleTagManager/GoogleTagManager.js");
 
			await _jsModule.InvokeVoidAsync("initialize" , _gtmOptions.GtmId, _gtmOptions.Attributes);

            _isInitialized = true;
		}

		/// <inheritdoc/>
		public async Task PushAsync(object data)
        {
            await InitializeAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("push", data);
            }
        }

		/// <inheritdoc/>
		public async Task PushEventAsync(string eventName, object? eventData = null)
        {
            await InitializeAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("pushEvent", eventName, eventData);
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
            await InitializeAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("pushPageViewEvent", _gtmOptions.PageViewEventName, _gtmOptions.PageViewUrlVariableName, url, additionalData);
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
