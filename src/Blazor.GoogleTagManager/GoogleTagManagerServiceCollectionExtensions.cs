using System;
using Blazor.GoogleTagManager.Interop;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Global

namespace Blazor.GoogleTagManager;

/// <summary>
/// Provides extension methods for adding Google Tag Manager (GTM) services to an <see cref="IServiceCollection"/>.
/// </summary>
public static class GoogleTagManagerServiceCollectionExtensions
{
    /// <summary>
    /// Adds Google Tag Manager (GTM) support. Use <see cref="IGoogleTagManager"/> to push data to <c>dataLayer</c> and/or <see cref="GoogleTagManagerPageViewTracker"/> to track page-views.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configureOptions">Options for Google Tag Manager.</param>
    /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddGoogleTagManager(this IServiceCollection services, Action<GoogleTagManagerOptions>? configureOptions)
    {
        if (OperatingSystem.IsBrowser())
        {
            services.AddSingleton<IGoogleTagManager, GoogleTagManager>();
            services.AddSingleton<IGoogleTagManagerInterop, GoogleTagManagerInterop>();
        }
        else
        {
            services.AddScoped<IGoogleTagManager, GoogleTagManager>();
            services.AddScoped<IGoogleTagManagerInterop, GoogleTagManagerInterop>();
        }

        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }

        return services;
    }

    /// <summary>
    /// Adds Google Tag Manager (GTM) support. Use <see cref="IGoogleTagManager"/> to push data to <c>dataLayer</c> and/or <see cref="GoogleTagManagerPageViewTracker"/> to track page-views.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="gtmId">The Google Tag Manager ID.</param>
    /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddGoogleTagManager(this IServiceCollection services, string gtmId)
    {
        return AddGoogleTagManager(services, options => { options.GtmId = gtmId; });
    }
}
