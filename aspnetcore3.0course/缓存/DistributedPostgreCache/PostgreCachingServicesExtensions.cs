using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedPostgreCache
{
    public static class PostgreCachingServicesExtensions
    {
        public static IServiceCollection AddDistributedPostgreCache(this IServiceCollection services, Action<PostgreCacheOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            services.AddOptions();
            AddPostgreCacheServices(services);
            services.Configure(setupAction);
            return services;
        }

        internal static void AddPostgreCacheServices(IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, PostgreCache>());
        }
    }
}
