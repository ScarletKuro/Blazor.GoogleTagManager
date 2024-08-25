using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;

namespace Blazor.GoogleTagManager;

/// <summary>
/// Support for <see href="https://developers.google.com/tag-manager/devguide">Google Tag Manager</see> - initialization and pushing data to data-layer.
/// </summary>
public interface IGoogleTagManager
{
    /// <summary>
    /// Indicates if GTM support is initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Indicates whenever the Google Tag Manager tracking is enabled.
    /// </summary>
    bool IsTackingEnabled { get; }

    /// <summary>
    /// Enables Google Tag Manager tracking.
    /// </summary>
    void EnableTracking();

    /// <summary>
    /// Disables Google Tag Manager tracking.
    /// </summary>
    void DisableTracking();

    /// <summary>
    /// Initializes the GTM support.
    /// Called automatically within first <c>Push</c> call (incl. <see cref="GoogleTagManagerPageViewTracker"/> calls).
    /// To be used explicitly only in those rare cases when you want to initialize GTM without pushing any data.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Push generic data to GTM data-layer (using regular JSON-serialization).
    /// </summary>
    Task PushAsync(object data);

    /// <summary>
    /// Push event to GTM data-layer.
    /// </summary>
    Task PushEventAsync(string eventName, object? eventData = null);

    /// <summary>
    /// Push page-view to GTM data-layer.
    /// Consider using <see cref="GoogleTagManagerPageViewTracker"/> instead of manual handling.
    /// </summary>
    Task PushPageViewAsync(object? additionalData = null);

    /// <summary>
    /// Used by <see cref="GoogleTagManagerPageViewTracker"/> to track location changes.
    /// </summary>
    Task PushPageViewAsync(LocationChangedEventArgs? args);
}