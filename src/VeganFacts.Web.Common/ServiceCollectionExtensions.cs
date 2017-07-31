using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganFacts.Web.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuth0(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<Auth0Options>(configuration.GetSection("VeganFacts:Auth0"));
        }
    }
}
