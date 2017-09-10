using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public class CloudBlobClientFactory : ICloudBlobClientFactory
    {
        private readonly StorageOptions _options;

        public CloudBlobClientFactory(
            IOptions<StorageOptions> options)
        {
            _options = options.Value;
        }

        public CloudBlobClient Create()
        {
            var storageAccount = CloudStorageAccount.Parse(_options.ConnectionString);
            return storageAccount.CreateCloudBlobClient();
        }
    }
}
