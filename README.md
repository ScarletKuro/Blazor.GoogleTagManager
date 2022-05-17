# Blazor.GoogleTagManager
This is a fork of [Havit.Blazor.GoogleTagManager](https://github.com/havit/Havit.Blazor/tree/master/Havit.Blazor.GoogleTagManager) but without Havit.Core, since for Blazor WASM every byte counts.
This library is trim friendly.

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
```CSharp
@inject IGoogleTagManager GoogleTagManager;

<button @onclick="OnButtonClick">Click</button>

@code {
  private async Task OnButtonClick(){
      await GoogleTagManager.PushAsync(new { @event = "button_click_sample_event" });
  }
}
```

### Additional Settings
You can add attributes, this can be useful for cookien consent
```CSharp
builder.Services.AddGoogleTagManager(options =>
{
      options.GtmId = "GTM-XXXXXXX";
      options.Attributes = new Dictionary<string, string>
      {
            { "data-consent-category", "google" }
      };
});
```
then in your scrip you will see following
```HTML
<script async="" src="https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX" data-consent-category="google"></script>
```
