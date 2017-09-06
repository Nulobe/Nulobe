using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class ResolutionContextExtensions
    {
        public static IServiceProvider GetServices(
            this ResolutionContext resolutionContext)
        {
            object serviceProvider;
            if (!resolutionContext.Options.Items.TryGetValue(Constants.ServiceProviderKey, out serviceProvider))
            {
                return null;
            }
            return serviceProvider as IServiceProvider;
        }

        public static TService GetRequiredService<TService>(
            this ResolutionContext resolutionContext)
        {
            var serviceProvider = resolutionContext.Options.Items[Constants.ServiceProviderKey] as IServiceProvider;
            return serviceProvider.GetRequiredService<TService>();
        }
    }
}
