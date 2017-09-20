using Nulobe.DocumentDb.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public class DocumentDbOptions : IDocumentDbConnectionSpec, IDocumentDbDatabaseSpec
    {
        public Uri ServiceEndpoint { get; set; }
        public string AuthorizationKey { get; set; }
        public string ReadOnlyAuthorizationKey { get; set; }
        public string DatabaseName { get; set; }
    }
}
