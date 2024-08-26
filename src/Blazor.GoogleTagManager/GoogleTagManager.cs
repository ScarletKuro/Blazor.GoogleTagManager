using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.GoogleTagManager.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;

namespace Blazor.GoogleTagManager;

/// <summary>
/// Implementation for <see cref="IGoogleTagManager"/>.
/// </summary>
public class GoogleTagManager : IGoogleTagManager
{
    private readonly GoogleTagManagerOptions _gtmOptions;
    private readonly NavigationManager _navigationManager;
    private readonly IGoogleTagManagerInterop _googleTagManagerInterop;

    /// <inheritdoc/>
    public bool IsInitialized { get; private set; }

    /// <inheritdoc/>
    public bool IsTackingEnabled { get; private set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleTagManager"/> class.
    /// </summary>
    /// <param name="gtmOptions">The options for Google Tag Manager.</param>
    /// <param name="navigationManager">The navigation manager.</param>
    /// <param name="googleTagManagerInterop">The interop service for Google Tag Manager.</param>
    public GoogleTagManager(
        IOptions<GoogleTagManagerOptions> gtmOptions,
        NavigationManager navigationManager,
        IGoogleTagManagerInterop googleTagManagerInterop)
    {
        _gtmOptions = gtmOptions.Value;
        _navigationManager = navigationManager;
        _googleTagManagerInterop = googleTagManagerInterop;
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

        await _googleTagManagerInterop.InitializeAsync(_gtmOptions.GtmId, _gtmOptions.Attributes, _gtmOptions.DebugToConsole);

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
        await _googleTagManagerInterop.PushAsync(data, _gtmOptions.DebugToConsole);
    }

    /// <inheritdoc/>
    public async Task PushEventAsync(string eventName, object? eventData = null)
    {
        if (!IsTackingEnabled)
        {
            return;
        }

        await InitializeAsync();
        await _googleTagManagerInterop.PushEventAsync(eventName, eventData, _gtmOptions.DebugToConsole);
    }

    /// <inheritdoc/>
    public Task PushPageViewAsync(object? additionalData = null) => PushPageViewCoreAsync(_navigationManager.Uri, additionalData);

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
        await _googleTagManagerInterop.PushPageViewAsync(_gtmOptions.PageViewEventName, _gtmOptions.PageViewUrlVariableName, url, additionalData, _gtmOptions.DebugToConsole);
    }
}
