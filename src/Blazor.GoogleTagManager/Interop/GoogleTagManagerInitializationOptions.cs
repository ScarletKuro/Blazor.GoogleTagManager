using System.Collections.Generic;

namespace Blazor.GoogleTagManager.Interop;

/// <summary>
/// Initialization options for Google Tag Manager JavaScript interop.
/// </summary>
public class GoogleTagManagerInitializationOptions
{
    /// <summary>
    /// Gets or sets the URL of the Google Tag Manager script host.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Google Tag Manager ID.
    /// </summary>
    public string GtmId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets HTML attributes to apply to the GTM script element.
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; } = new();

    /// <summary>
    /// Gets or sets additional query parameters for the GTM script URL.
    /// </summary>
    public Dictionary<string, string> QueryParameters { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether to write debug logs to the browser console.
    /// </summary>
    public bool DebugToConsole { get; set; }
}
