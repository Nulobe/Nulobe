using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganFacts.DocumentDb.Client
{
    public static class UriFactoryExtensions
    {
        public static Uri CreateDocumentDatabaseUri(IDocumentDbDatabaseSpec databaseSpec)
        {
            return UriFactory.CreateDatabaseUri(databaseSpec.DatabaseName);
        }

        public static Uri CreateDocumentCollectionUri(IDocumentDbCollectionSpec collectionSpec)
        {
            return UriFactory.CreateDocumentCollectionUri(collectionSpec.DatabaseName, collectionSpec.CollectionName);
        }

        public static Uri CreateDocumentUri(IDocumentDbCollectionSpec collectionSpec, string documentId)
        {
            return UriFactory.CreateDocumentUri(collectionSpec.DatabaseName, collectionSpec.CollectionName, documentId);
        }
    }
}
