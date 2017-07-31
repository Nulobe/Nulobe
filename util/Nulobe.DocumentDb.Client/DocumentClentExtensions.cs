using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.DocumentDb.Client
{
    public static class DocumentClientExtensions
    {
        public static Task<ResourceResponse<Document>> CreateDocumentAsync(
            this DocumentClient documentClient,
            IDocumentDbCollectionSpec collectionSpec,
            object document,
            RequestOptions options = null,
            bool disableAutomaticIdGeneration = false)
        {
            var documentCollectionUri = UriFactoryExtensions.CreateDocumentCollectionUri(collectionSpec);
            return documentClient.CreateDocumentAsync(documentCollectionUri, document, options, disableAutomaticIdGeneration);
        }

        public static IOrderedQueryable<ResourceType> CreateDocumentQuery<ResourceType>(
            this DocumentClient documentClient,
            IDocumentDbCollectionSpec collectionSpec,
            FeedOptions feedOptions = null)
        {
            var documentCollectionUri = UriFactoryExtensions.CreateDocumentCollectionUri(collectionSpec);
            return documentClient.CreateDocumentQuery<ResourceType>(documentCollectionUri, feedOptions);
        }

        public static async Task<TResourceType> ReadDocumentAsync<TResourceType>(
            this DocumentClient documentClient,
            IDocumentDbCollectionSpec collectionSpec,
            string documentId)
            where TResourceType : new()
        {
            var documentUri = UriFactoryExtensions.CreateDocumentUri(collectionSpec, documentId);
            return await documentClient.ReadDocumentAsync<TResourceType>(documentUri);
        }

        public static async Task<ResourceResponse<Document>> ReplaceDocumentAsync(
            this DocumentClient documentClient,
            IDocumentDbCollectionSpec collectionSpec,
            string documentId,
            object document,
            RequestOptions options = null)
        {
            var documentUri = UriFactoryExtensions.CreateDocumentUri(collectionSpec, documentId);
            return await documentClient.ReplaceDocumentAsync(documentUri.ToString(), document, options);
        }

        public static async Task EnsureDatabaseAsync(
            this DocumentClient documentClient,
            IDocumentDbDatabaseSpec databaseSpec)
        {
            await documentClient.CreateDatabaseIfNotExistsAsync(new Database()
            {
                Id = databaseSpec.DatabaseName
            });
        }

        public static async Task EnsureCollectionAsync(
            this DocumentClient documentClient,
            IDocumentDbCollectionSpec collectionSpec)
        {
            await documentClient.EnsureDatabaseAsync(collectionSpec);
            await documentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactoryExtensions.CreateDocumentDatabaseUri(collectionSpec),
                new DocumentCollection()
                {
                    Id = collectionSpec.CollectionName
                });
        }
    }
}
