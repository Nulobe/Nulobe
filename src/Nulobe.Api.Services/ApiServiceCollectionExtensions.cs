using Microsoft.Extensions.Configuration;
using Nulobe.Api.Services;
using Nulobe.DocumentDb.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();

            services.AddTransient<IFactService, FactService>();
            services.AddTransient<IFactQueryService, FactService>();

            services.ConfigureAuth0(configuration);
            services.Configure<FactServiceOptions>(configuration.GetSection("Nulobe:FactService"));

            return services;
        }
    }
}
