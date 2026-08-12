using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Xunit;

namespace Blazor.GoogleTagManager.SmokeTests;

public sealed class GoogleTagManagerSmokeTests : IAsyncLifetime
{
    private SmokeHostFactory? _factory;
    private string? _baseUrl;

    [Fact]
    public async Task AutomaticImportInitializesDataLayerAndPushesEvents()
    {
        Assert.NotNull(_baseUrl);

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.GotoAsync(_baseUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });

        await page.WaitForSelectorAsync("#status");
        await page.WaitForFunctionAsync("() => window.isGTM === true");
        await page.WaitForFunctionAsync("() => window.__smokeGtmLoaded === true");

        var events = await page.EvaluateAsync<string[]>("() => window.dataLayer.map(item => item.event)");
        var gtmScriptUrl = await page.EvaluateAsync<string>("() => document.querySelector('head script[src*=\"/gtm.js?id=GTM-SMOKE\"]')?.src ?? ''");

        Assert.Contains("gtm.js", events);
        Assert.Contains("pageview", events);
        Assert.Equal($"{_baseUrl}/gtm.js?id=GTM-SMOKE", gtmScriptUrl);

        await page.ClickAsync("#push-event");
        await page.WaitForFunctionAsync("() => window.dataLayer.some(item => item.event === 'smoke_click')");

        var updatedEvents = await page.EvaluateAsync<string[]>("() => window.dataLayer.map(item => item.event)");
        Assert.Contains("smoke_click", updatedEvents);
    }

    public async Task InitializeAsync()
    {
        var port = GetFreeTcpPort();
        _baseUrl = $"http://127.0.0.1:{port}";
        _factory = new SmokeHostFactory(_baseUrl);
        _factory.UseKestrel(port);
        _factory.StartServer();

        try
        {
            await WaitForHostAsync($"{_baseUrl}/ready");
        }
        catch
        {
            throw new InvalidOperationException($"Smoke host failed to start at {_baseUrl}.");
        }
    }

    public Task DisposeAsync()
    {
        _factory?.Dispose();
        return Task.CompletedTask;
    }

    private static async Task WaitForHostAsync(string baseUrl)
    {
        using var httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(2)
        };

        var deadline = DateTimeOffset.UtcNow.AddSeconds(30);

        while (DateTimeOffset.UtcNow < deadline)
        {
            try
            {
                using var response = await httpClient.GetAsync(baseUrl);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
            }

            await Task.Delay(250);
        }

        throw new TimeoutException($"Timed out waiting for smoke host at {baseUrl}.");
    }

    private static int GetFreeTcpPort()
    {
        using var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        return ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
    }

    private sealed class SmokeHostFactory(string gtmUrl) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("GTM_URL", gtmUrl);
        }
    }
}
