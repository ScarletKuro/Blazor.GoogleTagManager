using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Blazor.GoogleTagManager
{
	public class GoogleTagManagerPageViewTracker : ComponentBase, IDisposable
    {
        [Inject] 
        protected NavigationManager NavigationManager { get; set; } = null!;

		[Inject] 
        protected IGoogleTagManager GoogleTagManager { get; set; } = null!;

		private LocationChangedEventArgs? _locationChangedEventArgsToReportOnAfterRenderAsync;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			NavigationManager.LocationChanged += OnLocationChanged;
		}

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

		private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
		{
			_locationChangedEventArgsToReportOnAfterRenderAsync = args;
			StateHasChanged();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				NavigationManager.LocationChanged -= OnLocationChanged;
			}
		}
	}
}
