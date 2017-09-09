using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.CosmosDataMigration
{
    public class CosmosSinkOptions
    {
        public Uri ServiceEndpoint { get; set; }
        public string AuthorizationKey { get; set; }
    }
}
