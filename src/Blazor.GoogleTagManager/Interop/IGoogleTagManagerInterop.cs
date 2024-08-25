using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.GoogleTagManager.Interop;

public interface IGoogleTagManagerInterop
{
    Task InitializeAsync(string gtmId, Dictionary<string, string> attributes, bool debugToConsole);

    Task PushAsync(object data, bool debugToConsole);

    Task PushEventAsync(string eventName, object? eventData, bool debugToConsole);

    Task PushPageViewAsync(string pageViewEventName, string pageViewUrlVariableName, string url, object? additionalData, bool debugToConsole);
}