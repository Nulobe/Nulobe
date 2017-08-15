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
            IDocumentDbDatabaseSpec documentDbDatabaseSpec,
            string collectionName,
            object document,
            RequestOptions options = null,
            bool disableAutomaticIdGeneration = false)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(documentDbDatabaseSpec.DatabaseName, collectionName);
            return documentClient.CreateDocumentAsync(uri, document, options, disableAutomaticIdGeneration);
        }

        public static Task<ResourceResponse<Document>> DeleteDocumentAsync(
            this DocumentClient documentClient,
            IDocumentDbDatabaseSpec documentDbDatabaseSpec,
            string collectionName,
            string id,
            RequestOptions options = null)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(documentDbDatabaseSpec.DatabaseName, collectionName);
            return documentClient.DeleteDocumentAsync(uri, options);
        }

        public static IQueryable<ResourceType> CreateDocumentQuery<ResourceType>(
            this DocumentClient documentClient,
            IDocumentDbDatabaseSpec documentDbDatabaseSpec,
            string collectionName,
            SqlQuerySpec sqlQuery,
            FeedOptions feedOptions = null)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(documentDbDatabaseSpec.DatabaseName, collectionName);
            return documentClient.CreateDocumentQuery<ResourceType>(uri, sqlQuery, feedOptions);
        }

        public static async Task<TResult> ReadDocumentAsync<TResult>(
            this DocumentClient documentClient,
            IDocumentDbDatabaseSpec documentDbDatabaseSpec,
            string collectionName,
            string id) where TResult : new()
        {
            var uri = UriFactory.CreateDocumentUri(documentDbDatabaseSpec.DatabaseName, collectionName, id);
            var documentResponse = await documentClient.ReadDocumentAsync<TResult>(uri);
            return documentResponse.Document;
        }

        public static async Task<ResourceResponse<Document>> ReplaceDocumentAsync(
            this DocumentClient documentClient,
            IDocumentDbDatabaseSpec documentDbDatabaseSpec,
            string collectionName,
            string documentId,
            object document,
            RequestOptions options = null)
        {
            var documentUri = UriFactory.CreateDocumentUri(documentDbDatabaseSpec.DatabaseName, collectionName, documentId);
            return await documentClient.ReplaceDocumentAsync(documentUri, document, options);
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
            IDocumentDbDatabaseSpec documentDbDatabaseSpec,
            string collectionName)
        {
            await documentClient.EnsureDatabaseAsync(documentDbDatabaseSpec);
            await documentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(documentDbDatabaseSpec.DatabaseName),
                new DocumentCollection()
                {
                    Id = collectionName
                });
        }
    }
}
