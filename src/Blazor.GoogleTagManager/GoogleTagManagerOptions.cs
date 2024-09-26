using System.Collections.Generic;

namespace Blazor.GoogleTagManager;

/// <summary>
/// Options for <see cref="GoogleTagManager"/>.
/// </summary>
public class GoogleTagManagerOptions
{
    /// <summary>
    /// Gets or sets the URL of the Google Tag Manager script.
    /// For more information, see <see href="https://developers.google.com/tag-platform/tag-manager/server-side/custom-domain">Custom Domain for Google Tag Manager</see>.
    /// <para/>
    /// The default value is <c>https://www.googletagmanager.com</c>.
    /// </summary>
    /// <remarks>
    /// Do not add a trailing slash '/' at the end of the URL.
    /// </remarks>
    public string Url { get; set; } = "https://www.googletagmanager.com";

    /// <summary>
    /// Gets or sets the GTM-ID.
    /// The default value is an empty string.
    /// </summary>
    public string GtmId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Google Tag Manager script attributes.
    /// The default value is an empty dictionary.
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; } = new();

    /// <summary>
    /// Gets or sets the name of the event pushed when a page-view is tracked.
    /// The default value is <c>"virtualPageView"</c>.
    /// </summary>
    public string PageViewEventName { get; set; } = "virtualPageView";

    /// <summary>
    /// Gets or sets the name of the variable to be used for the URL when a page-view is tracked.
    /// The default value is <c>"pageUrl"</c>.
    /// </summary>
    public string PageViewUrlVariableName { get; set; } = "pageUrl";

    /// <summary>
    /// Gets or sets a value indicating whether to print logs in the browser console.
    /// Do not use in production.
    /// The default value is <c>false</c>.
    /// </summary>
    public bool DebugToConsole { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to import the JavaScript <c>'_content/Blazor.GoogleTagManager/GoogleTagManager.js'</c> file automatically.
    /// <para>
    /// If you disable this option, you must manually import your own JavaScript via the <c>script</c> tag.
    /// </para>
    /// The default value is <c>true</c>.
    /// </summary>
    public bool ImportJsAutomatically { get; set; } = true;
}
