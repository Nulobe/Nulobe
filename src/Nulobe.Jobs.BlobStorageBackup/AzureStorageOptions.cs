using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Jobs.BlobStorageBackup
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; set; }

        public AzureStorageAuthOptions Auth { get; set; }
    }

    public class AzureStorageAuthOptions
    {
        public string AccountName { get; set; }

        public string AccountKey { get; set; }
    }
}
