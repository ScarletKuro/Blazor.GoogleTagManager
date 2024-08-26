using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.GoogleTagManager.Interop;

/// <summary>
/// Provides JavaScript interop for Google Tag Manager, including initialization and pushing data to the data layer.
/// </summary>
public interface IGoogleTagManagerInterop
{
    /// <summary>
    /// Initializes the Google Tag Manager with the specified GTM ID, attributes, and debug option.
    /// </summary>
    /// <param name="gtmId">The Google Tag Manager ID.</param>
    /// <param name="attributes">A dictionary of attributes to initialize with.</param>
    /// <param name="debugToConsole">Indicates whether to output debug information to the console.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InitializeAsync(string gtmId, Dictionary<string, string> attributes, bool debugToConsole);

    /// <summary>
    /// Pushes generic data to the GTM data layer.
    /// </summary>
    /// <param name="data">The data to push.</param>
    /// <param name="debugToConsole">Indicates whether to output debug information to the console.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PushAsync(object data, bool debugToConsole);

    /// <summary>
    /// Pushes an event to the GTM data layer.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <param name="eventData">Optional event data.</param>
    /// <param name="debugToConsole">Indicates whether to output debug information to the console.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PushEventAsync(string eventName, object? eventData, bool debugToConsole);

    /// <summary>
    /// Pushes a page view to the GTM data layer.
    /// </summary>
    /// <param name="pageViewEventName">The name of the page view event.</param>
    /// <param name="pageViewUrlVariableName">The variable name for the page view URL.</param>
    /// <param name="url">The URL of the page view.</param>
    /// <param name="additionalData">Optional additional data.</param>
    /// <param name="debugToConsole">Indicates whether to output debug information to the console.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PushPageViewAsync(string pageViewEventName, string pageViewUrlVariableName, string url, object? additionalData, bool debugToConsole);
}
