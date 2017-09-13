using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public class CloudStorageClientFactory : ICloudStorageClientFactory
    {
        private readonly StorageOptions _options;

        public CloudStorageClientFactory(
            IOptions<StorageOptions> options)
        {
            _options = options.Value;
        }

        public CloudBlobClient CreateBlobClient() => GetStorageAccount().CreateCloudBlobClient();

        public CloudTableClient CreateTableClient() => GetStorageAccount().CreateCloudTableClient();


        #region Helpers

        private CloudStorageAccount GetStorageAccount() => CloudStorageAccount.Parse(_options.ConnectionString);

        #endregion
    }
}
