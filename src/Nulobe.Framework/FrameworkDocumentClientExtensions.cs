using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Nulobe.DocumentDb.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public static class FrameworkDocumentClientExtensions
    {
        private const string DatabaseName = "Nulobe";
        private const string FactCollectionName = "Facts";

        public static IQueryable<T> CreateFactDocumentQuery<T>(this DocumentClient client, SqlQuerySpec sqlQuery, FeedOptions feedOptions = null)
            => client.CreateDocumentQuery<T>(GetFactCollectionUri(), sqlQuery, feedOptions);

        public static Task<ResourceResponse<Document>> CreateFactDocumentAsync(this DocumentClient client, object document, RequestOptions options = null, bool disableAutomaticIdGeneration = false)
            => client.CreateDocumentAsync(GetFactCollectionUri(), document, options, disableAutomaticIdGeneration);

        public static async Task<TResult> ReadFactDocumentAsync<TResult>(this DocumentClient client, string id) where TResult : new()
        {
            DocumentResponse<TResult> documentResponse = null;
            try
            {
                documentResponse = await client.ReadDocumentAsync<TResult>(GetFactDocumentUri(id));
            }
            catch (DocumentClientException ex)
            {
                if (ex.Error.Code == "NotFound")
                {
                    throw new DocumentNotFoundException($"Could not locate the document with ID {id}", ex);
                }
                throw ex;
            }

            return documentResponse.Document;
        }

        public static Task<ResourceResponse<Document>> ReplaceFactDocumentAsync(this DocumentClient client, string id, object document, RequestOptions options = null)
            => client.ReplaceDocumentAsync(GetFactDocumentUri(id), document, options);

        public static Task<ResourceResponse<Document>> DeleteFactDocumentAsync(this DocumentClient client, string id, RequestOptions options = null)
            => client.DeleteDocumentAsync(GetFactDocumentUri(id), options);

        public static Task EnsureFactDatabaseAsync(this DocumentClient client)
            => client.CreateDatabaseIfNotExistsAsync(new Database() { Id = DatabaseName });

        public static async Task EnsureFactCollectionAsync(this DocumentClient client)
        {
            await client.EnsureFactDatabaseAsync();
            await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DatabaseName),
                new DocumentCollection()
                {
                    Id = FactCollectionName
                });
        }

        private static Uri GetFactCollectionUri() => UriFactory.CreateDocumentCollectionUri(DatabaseName, FactCollectionName);

        private static Uri GetFactDocumentUri(string id) => UriFactory.CreateDocumentUri(DatabaseName, FactCollectionName, id);
    }
}
