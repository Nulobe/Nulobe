using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nulobe.DocumentDb.Client;

namespace Nulobe.Api.Core.Facts
{
    public class FactServiceOptions
    {
        public string FactCollectionName { get; set; }

        public string FactAuditCollectionName { get; set; }

        public int MaxSourceCount { get; set; }
    }
}
