﻿using Microsoft.Extensions.Configuration;
using Nulobe.Api.Core.Events;
using Nulobe.Api.Core.Facts;
using Nulobe.Api.Core.Sources;
using Nulobe.Api.Core.Sources.Impl;
using Nulobe.Api.Core.Tags;
using Nulobe.DocumentDb.Client;
using Nulobe.Microsoft.WindowsAzure.Storage;
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

            services.AddTransient<SourcePipeline>();
            services.AddTransient<ISourceHandler, LegacySourceHandler>();
            services.AddTransient<ISourceHandler, NulobeSourceHandler>();
            services.AddTransient<ISourceHandler, CitationNeededSourceHandler>();
            services.AddTransient<ISourceHandler, ApaSourceHandler>();
            services.AddTransient<ISourceFieldDictionary, SourceFieldsDictionary>();

            services.AddTransient<SourcePipeline>();
            services.AddTransient<ISourceHandler, DoiSourceHandler>();

            services.AddTransient<ITagQueryService, TagQueryService>();
            services.AddSingleton<ITagMemoryRepository, TagMemoryRepository>();
            services.AddTransient<IFlagEventService, FlagEventService>();
            services.AddTransient<IVoteEventService, VoteEventService>();

            services.AddTransient<ICloudStorageClientFactory, CloudStorageClientFactory>();

            services.Configure<FactServiceOptions>(configuration.GetSection("Nulobe:FactService"));

            return services;
        }
    }
}
