using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Documents.Client
{
    public static class DocumentClientEnsuredExtensions {

        public static async Task<T> ExecuteEnsuredStoredProcedureAsync<T>(
                this DocumentClient client,
                string databaseId,
                string collectionId,
                string storedProcedurePath,
                dynamic[] parameters)
        {
            var storedProcedureId = GetStoredProcedureId(storedProcedurePath);

            bool isCollectionEnsured = false;
            bool isStoredProcedureEnsured = false;
            var storedProcedureUri = UriFactory.CreateStoredProcedureUri(databaseId, collectionId, storedProcedureId);
            while (true)
            {
                try
                {
                    var storedProcedureResult = await client.ExecuteStoredProcedureAsync<T>(storedProcedureUri, parameters);
                    return storedProcedureResult.Response;
                }
                catch (DocumentClientException ex)
                {
                    if (ex.Error.Code.Equals("NotFound", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!isCollectionEnsured)
                        {
                            await client.EnsureCollectionAsync(databaseId, collectionId);
                            isCollectionEnsured = true;
                        }
                        else if (!isStoredProcedureEnsured)
                        {
                            await client.EnsureStoredProcedureAsync(databaseId, collectionId, storedProcedurePath, ensureDocumentCollection: false);
                            isStoredProcedureEnsured = true;
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static async Task EnsureCollectionAsync(
            this DocumentClient client,
            string databaseId,
            string collectionId)
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database()
            {
                Id = databaseId
            });

            await client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection()
                {
                    Id = collectionId
                });
        }

        public static async Task EnsureStoredProcedureAsync(
            this DocumentClient client,
            string databaseId,
            string collectionId,
            string storedProcedurePath,
            bool ensureDocumentCollection = false)
        {
            if (ensureDocumentCollection)
            {
                await client.EnsureCollectionAsync(databaseId, collectionId);
            }

            string storedProcedureId = GetStoredProcedureId(storedProcedurePath);
            try
            {
                var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (var fs = File.OpenRead(Path.Combine(executingDirectory, storedProcedurePath)))
                using (var sr = new StreamReader(fs))
                {
                    await client.CreateStoredProcedureAsync(
                        UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                        new StoredProcedure()
                        {
                            Id = storedProcedureId,
                            Body = await sr.ReadToEndAsync()
                        });
                }
            }
            catch (DocumentClientException ex)
            {
                // If the code is "Conflict" it means the stored procedure already existed
                if (!ex.Error.Code.Equals("Conflict", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw;
                }
            }
        }

        private static string GetStoredProcedureId(string storedProcedurePath) => Path.GetFileNameWithoutExtension(storedProcedurePath);
    }
}
