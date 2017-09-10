using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public interface ICloudBlobClientFactory
    {
        CloudBlobClient Create();
    }
}
