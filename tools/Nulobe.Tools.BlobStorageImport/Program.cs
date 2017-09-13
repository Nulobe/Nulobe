using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Nulobe.CosmosDataMigration;
using Nulobe.DocumentDb.Client;
using Nulobe.Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Tools.BlobStorageImport
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var environmentName = "Development";
#else
            var environmentName = "Production";
#endif

            var configuration = new ConfigurationBuilder()
                .AddConfigurationSources<Program>(MockHostingEnvironment.For(environmentName))
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddOptions();
            services.ConfigureDocumentDb(configuration);
            services.ConfigureStorage(configuration);

            services.AddTransient<CosmosDataMigrationToolClient>();
            services.AddTransient<ICloudStorageClientFactory, CloudStorageClientFactory>();
            services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();

            var serviceProvider = services.BuildServiceProvider();
            var blobStorageBackup = ActivatorUtilities.CreateInstance<BlobStorageImportService>(serviceProvider);
            blobStorageBackup.RunAsync().Wait();
        }

        private class MockHostingEnvironment : IHostingEnvironment
        {
            public static MockHostingEnvironment For(string environment) => new MockHostingEnvironment()
            {
                EnvironmentName = environment
            };

            public string EnvironmentName { get; set; }
            public string ApplicationName { get; set; }
            public string WebRootPath { get; set; }
            public IFileProvider WebRootFileProvider { get; set; }
            public string ContentRootPath { get; set; }
            public IFileProvider ContentRootFileProvider { get; set; }
        }
    }
}
