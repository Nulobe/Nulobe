using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using Nulobe.CosmosDataMigration;
using Nulobe.Framework;
using Nulobe.Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Jobs.BlobStorageBackup
{
    public class BlobStorageBackupService
    {
        private readonly StorageOptions _storageOptions;
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly CosmosDataMigrationToolClient _cosmosDataMigrationToolClient;
        private readonly ICloudStorageClientFactory _cloudBlobClientFactory;

        public BlobStorageBackupService(
            IOptions<StorageOptions> storageOptions,
            IOptions<DocumentDbOptions> documentDbOptions,
            CosmosDataMigrationToolClient cosmosDataMigrationToolClient,
            ICloudStorageClientFactory cloudBlobClientFactory)
        {
            _storageOptions = storageOptions.Value;
            _documentDbOptions = documentDbOptions.Value;
            _cosmosDataMigrationToolClient = cosmosDataMigrationToolClient;
            _cloudBlobClientFactory = cloudBlobClientFactory;
        }

        public async Task RunAsync()
        {
            _cosmosDataMigrationToolClient.Transfer(
                new CosmosSink(new CosmosSinkOptions()
                {
                    ServiceEndpoint = _documentDbOptions.ServiceEndpoint,
                    AuthorizationKey = _documentDbOptions.AuthorizationKey
                }),
                new JsonFileDataSink(new JsonFileDataSinkOptions()
                {
                    Directory = Directory.GetCurrentDirectory()
                }),
                _documentDbOptions.DatabaseName,
                "Facts");

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Nulobe.Facts.json");

            using (var fs = File.OpenRead(outputPath))
            {
                var containerClient = _cloudBlobClientFactory.CreateBlobClient();
                var container = await containerClient.GetCloudBlobContainerAsync("prodcopy", BlobContainerPublicAccessType.Blob);

                var now = DateTime.UtcNow;
                await container.UploadAsync(new Microsoft.WindowsAzure.Storage.CloudBlob()
                {
                    Path = Path.Combine(new object[]
                    {
                        now.Year,
                        now.Month,
                        now.Day,
                        "Nulobe.Facts.json"
                    }.Select(o => o.ToString()).ToArray()),
                    ContentType = "application/json",
                    Stream = fs
                });
            }

            File.Delete(outputPath);
        }
    }
}
