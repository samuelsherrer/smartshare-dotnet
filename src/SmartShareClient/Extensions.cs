using System;
using SmartShareClient.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;

namespace SmartShareClient
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartShare(this IServiceCollection services, Action<IServiceProvider, SmartShareOptions> configureOptions)
        {
            services.AddOptions<SmartShareOptions>().Configure<IServiceProvider>((options, resolver) => configureOptions(resolver, options))
                .PostConfigure(options =>
                {
                    if (string.IsNullOrWhiteSpace(options.Endpoint))
                        throw new ArgumentNullException(nameof(options.Endpoint));

                    if (string.IsNullOrWhiteSpace(options.ClientId))
                        throw new ArgumentNullException(nameof(options.ClientId));

                    if (string.IsNullOrWhiteSpace(options.ClientKey))
                        throw new ArgumentNullException(nameof(options.ClientKey));

                    if (string.IsNullOrWhiteSpace(options.User))
                        throw new ArgumentNullException(nameof(options.User));

                    if (string.IsNullOrWhiteSpace(options.Password))
                        throw new ArgumentNullException(nameof(options.Password));
                });

            return services.AddTransient<ISmartShare, SmartShare>();
        }

        public static IServiceCollection AddSmartShare(this IServiceCollection services, Action<SmartShareOptions> configureOptions)
        {
            return services.AddSmartShare((_, options) => configureOptions(options));
        }
    }
}
