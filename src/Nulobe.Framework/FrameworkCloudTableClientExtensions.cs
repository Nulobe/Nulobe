using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.Storage.Table
{
    public static class FrameworkCloudTableClientExtensions
    {
        public static CloudTable GetTagsTableReference(this CloudTableClient client) => client.GetTableReference("tags");
    }
}
