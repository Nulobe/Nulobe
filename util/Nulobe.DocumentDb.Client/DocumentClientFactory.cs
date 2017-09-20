using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.DocumentDb.Client
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly DocumentDbOptions _options;

        public DocumentClientFactory(
            IOptions<DocumentDbOptions> options)
        {
            _options = options.Value;
        }

        DocumentClient IDocumentClientFactory.Create(bool readOnly) {
            var client = DocumentClientAccount.Parse(readOnly ? _options.ReadOnlyConnectionString : _options.ConnectionString);
            return client;
        }
    }
}