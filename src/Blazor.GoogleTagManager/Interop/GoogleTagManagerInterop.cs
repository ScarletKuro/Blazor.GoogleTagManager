using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System;

namespace Blazor.GoogleTagManager.Interop;

internal class GoogleTagManagerInterop : IGoogleTagManagerInterop, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private Task<IJSObjectReference>? _module;
    private readonly IOptions<GoogleTagManagerOptions> _options;

    private Task<IJSObjectReference> Module => _module ??= _jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", ["./_content/Blazor.GoogleTagManager/GoogleTagManager.js"])
        .AsTask();

    public GoogleTagManagerInterop(IOptions<GoogleTagManagerOptions> gtmOptions, IJSRuntime jsRuntime)
    {
        _options = gtmOptions;
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync(string gtmId, Dictionary<string, string> attributes, bool debugToConsole)
    {
        if (_options.Value.ImportJsAutomatically)
        {
            var module = await Module;

            await module.InvokeVoidAsync("initialize", gtmId, attributes, debugToConsole);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("initialize", gtmId, attributes, debugToConsole);
        }
    }

    public async Task PushAsync(object data, bool debugToConsole)
    {
        if (_options.Value.ImportJsAutomatically)
        {
            var module = await Module;

            await module.InvokeVoidAsync("push", data, debugToConsole);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("push", data, debugToConsole);
        }
    }

    public async Task PushEventAsync(string eventName, object? eventData, bool debugToConsole)
    {
        if (_options.Value.ImportJsAutomatically)
        {
            var module = await Module;

            await module.InvokeVoidAsync("pushEvent", eventName, eventData, debugToConsole);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("pushEvent", eventName, eventData, debugToConsole);
        }
    }

    public async Task PushPageViewAsync(string pageViewEventName, string pageViewUrlVariableName, string url, object? additionalData, bool debugToConsole)
    {
        if (_options.Value.ImportJsAutomatically)
        {
            var module = await Module;

            await module.InvokeVoidAsync("pushPageViewEvent", pageViewEventName, pageViewUrlVariableName, url, additionalData, debugToConsole);
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("pushPageViewEvent", pageViewEventName, pageViewUrlVariableName, url, additionalData, debugToConsole);
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            var module = await Module;

            await module.DisposeAsync();
        }
    }
}