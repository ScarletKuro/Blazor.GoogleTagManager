# <img src="https://github.com/ScarletKuro/Blazor.GoogleTagManager/blob/main/google-tag-mager-icon.png" height="26" /> Blazor.GoogleTagManager
[![Nuget](https://img.shields.io/nuget/v/Blazor.GoogleTagManager?color=ff4081&logo=nuget)](https://www.nuget.org/packages/Blazor.GoogleTagManager/)
[![Nuget](https://img.shields.io/nuget/dt/Blazor.GoogleTagManager?color=ff4081&label=nuget%20downloads&logo=nuget)](https://www.nuget.org/packages/Blazor.GoogleTagManager/)
[![GitHub](https://img.shields.io/github/license/ScarletKuro/Blazor.GoogleTagManager?color=594ae2&logo=github)](https://github.com/ScarletKuro/Blazor.GoogleTagManager/blob/main/LICENSE)

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
Add the following component to your **MainLayout.razor**
```HTML
<GoogleTagManagerPageViewTracker />
```
**NB!** There is no need to add `_content/Blazor.GoogleTagManager/GoogleTagManager.js` in indedx.html / _Host.cshtml, the script is imported automatically. 

## Sample Usage
For general use case, please refer to [google tutorials](https://support.google.com/tagmanager/answer/6103696?hl=en) or any other learning materials.

### Manual Push
Only if you need to trigger custom events from the code.

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

## Additional Settings
You can add attributes, this can be useful for cookie consent
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
<script async src="https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX" data-consent-category="google"></script>
```
Other useful links for consent settings: [link1](https://support.google.com/tagmanager/answer/10718549) [link2](https://developers.google.com/tag-platform/tag-manager/templates/consent-apis)


## Troubleshooting
If nothing happens, even a simple `pageview` event, and you are sure you configured the library and Google Tag Manager correctly, then check if adblocker/firewall doesn't block the Google Tag Manager script. For example, AdGuard by default can remove tracking scripts.
Try to use console and other tools to make sure that the script(`https://www.googletagmanager.com/gtm.js?id=GTM-XXXXXXX`) is present on the page and that you have access to dataLayer object.

Also, there is [debug](https://support.google.com/tagmanager/answer/6107056?hl=en) / [tag assistant](https://tagassistant.google.com/) feature for Google Tag Manager that will always help you to debug your triggers and show that your GTM is hooked up properly.

## Not Supported Scenarios
There is no support for the [Content Security Policy](https://developers.google.com/tag-platform/tag-manager/web/csp) out of the box, as that would require additional JavaScript modification.

There is also no support for [renaming](https://developers.google.com/tag-platform/tag-manager/web/datalayer#tag-manager) the dataLayer object for the Google Tag Manager.
