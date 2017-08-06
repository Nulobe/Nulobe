using Microsoft.Extensions.Configuration;
using Nulobe.Api.Core.Facts;
using Nulobe.Api.Core.Tags;
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
        public static IServiceCollection AddCoreApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();

            services.AddTransient<IFactService, FactService>();
            services.AddTransient<IFactQueryService, FactService>();
            services.AddTransient<ITagQueryService, TagQueryService>();

            services.Configure<FactServiceOptions>(configuration.GetSection("Nulobe:FactService"));

            return services;
        }
    }
}
