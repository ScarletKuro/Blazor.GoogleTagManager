using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Blazor.GoogleTagManager;

/// <summary>
/// Tracks page views and reports them to Google Tag Manager.
/// </summary>
public class GoogleTagManagerPageViewTracker : ComponentBase, IDisposable
{
    private LocationChangedEventArgs? _locationChangedEventArgsToReportOnAfterRenderAsync;

    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/> used to manage navigation.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="IGoogleTagManager"/> used to push page views to Google Tag Manager.
    /// </summary>
    [Inject]
    protected IGoogleTagManager GoogleTagManager { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender || _locationChangedEventArgsToReportOnAfterRenderAsync is not null)
        {
            var argsToReport = _locationChangedEventArgsToReportOnAfterRenderAsync;
            _locationChangedEventArgsToReportOnAfterRenderAsync = null;
            await GoogleTagManager.PushPageViewAsync(argsToReport);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Handles location changes and triggers a state change.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The location changed event arguments.</param>
    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        _locationChangedEventArgsToReportOnAfterRenderAsync = args;
        StateHasChanged();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources used by the component.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from the Dispose method.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
