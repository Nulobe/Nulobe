using Nulobe.DocumentDb.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class EventServiceOptions : IDocumentDbConnectionSpec, IDocumentDbCollectionSpec
    {
        public Uri ServiceEndpoint { get; set; }
        public string AuthorizationKey { get; set; }
        public string CollectionName { get; set; }
        public string DatabaseName { get; set; }
    }
}
