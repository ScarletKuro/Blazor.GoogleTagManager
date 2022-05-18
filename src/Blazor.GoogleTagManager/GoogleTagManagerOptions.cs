using System.Collections.Generic;

namespace Blazor.GoogleTagManager
{
    /// <summary>
    /// Options for <see cref="GoogleTagManager"/>.
    /// </summary>
    public class GoogleTagManagerOptions
    {
        /// <summary>
        /// GTM-ID
        /// </summary>
        public string GtmId { get; set; } = string.Empty;

        public Dictionary<string, string> Attributes { get; set; } = new();

        /// <summary>
        /// Name of the event pushed when page-view is tracked.
        /// </summary>
        public string PageViewEventName { get; set; } = "virtualPageView";

        /// <summary>
        /// Name of the variabel to be used for URL when page-view is tracked.
        /// </summary>
        public string PageViewUrlVariableName { get; set; } = "pageUrl";

        /// <summary>
        /// Allows to print logs in the browser console. Do not use in production.
        /// </summary>
        public bool DebugToConsole { get; set; }
    }
}