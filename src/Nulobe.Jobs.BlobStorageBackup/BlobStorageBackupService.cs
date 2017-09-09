using Microsoft.Extensions.Options;
using Nulobe.CosmosDataMigration;
using Nulobe.Framework;
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
        private readonly AzureStorageOptions _storageOptions;
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly AzureBlobStorageService _azureBlobStorageService;
        private readonly CosmosDataMigrationToolClient _cosmosDataMigrationToolClient;

        public BlobStorageBackupService(
            IOptions<AzureStorageOptions> storageOptions,
            IOptions<DocumentDbOptions> documentDbOptions,
            AzureBlobStorageService azureBlobStorageService,
            CosmosDataMigrationToolClient cosmosDataMigrationToolClient)
        {
            _storageOptions = storageOptions.Value;
            _documentDbOptions = documentDbOptions.Value;
            _azureBlobStorageService = azureBlobStorageService;
            _cosmosDataMigrationToolClient = cosmosDataMigrationToolClient;
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
                var now = DateTime.UtcNow;
                await _azureBlobStorageService.UploadAsync(new FileStorageData()
                {
                    ContentType = "application/json",
                    KeyParts = new object[] { now.Year, now.Month, now.Day }.Select(k => k.ToString()),
                    Length = fs.Length,
                    Name = "Nulobe.Facts.json",
                    Stream = fs
                });
            }

            File.Delete(outputPath);
        }
    }
}
