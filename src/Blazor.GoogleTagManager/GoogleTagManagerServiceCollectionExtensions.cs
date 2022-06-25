using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.GoogleTagManager
{
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
            {
                services.AddSingleton<IGoogleTagManager, GoogleTagManager>();
            }
            else
            {
                services.AddScoped<IGoogleTagManager, GoogleTagManager>();
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
        /// <param name="gtmId">GTM Id./</param>
        /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddGoogleTagManager(this IServiceCollection services, string gtmId)
        {
            return AddGoogleTagManager(services, options => { options.GtmId = gtmId; });
        }
    }
}