using Microsoft.Extensions.Configuration;
using Nulobe.Api.Core.Events;
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
            services.AddTransient<IFactQueryService, FactQueryService>();
            services.AddTransient<ITagQueryService, TagQueryService>();
            services.AddTransient<ITagMemoryRepository, TagMemoryRepository>();
            services.AddTransient<IFlagEventService, FlagEventService>();
            services.AddTransient<IVoteEventService, VoteEventService>();

            services.Configure<FactServiceOptions>(configuration.GetSection("Nulobe:FactService"));
            services.Configure<EventServiceOptions>(configuration.GetSection("Nulobe:EventService"));

            return services;
        }
    }
}
