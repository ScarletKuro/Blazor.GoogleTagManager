# Blazor.GoogleTagManager
This is a fork of [Havit.Blazor.GoogleTagManager](https://github.com/havit/Havit.Blazor/tree/master/Havit.Blazor.GoogleTagManager) but without Havit.Core, and for Blazor WASM every byte counts.

## Getting Started
### Register Services
Blazor ServerSide or WASM
```CSharp
builder.Services.AddGoogleTagManager(options =>
{
      options.GtmId = "GTM-XXXXXXX";
});
```

### Add Imports
After the package is added, you need to add the following in your **_Imports.razor**
```CSharp
@using Blazor.GoogleTagManager;
```

### Add Components
Add the following components to your **MainLayout.razor**
```HTML
<GoogleTagManagerPageViewTracker />
```

## Sample Usage
### Manual Push
In the razor component
```HTML
@inject IGoogleTagManager GoogleTagManager;

<button @onclick="OnButtonClick">Click</button>

@code {
  private async Task OnButtonClick(){
      await GoogleTagManager.PushAsync(new { @event = "button_click_sample_event" });
  }
}
```
