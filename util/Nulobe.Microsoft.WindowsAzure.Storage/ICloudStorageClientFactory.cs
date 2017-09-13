using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public interface ICloudStorageClientFactory
    {
        CloudBlobClient CreateBlobClient();

        CloudTableClient CreateTableClient();
    }
}
