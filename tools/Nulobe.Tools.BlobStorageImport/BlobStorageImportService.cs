using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using Nulobe.Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Tools.BlobStorageImport
{
    public class BlobStorageImportService
    {
        private readonly ICloudStorageClientFactory _cloudBlobClientFactory;
        private readonly IDocumentClientFactory _documentDbClientFactory;
        private readonly DocumentDbOptions _documentDbOptions;

        public BlobStorageImportService(
            ICloudStorageClientFactory cloudBlobClientFactory,
            IDocumentClientFactory documentDbClientFactory,
            IOptions<DocumentDbOptions> documentDbOptions)
        {
            _cloudBlobClientFactory = cloudBlobClientFactory;
            _documentDbClientFactory = documentDbClientFactory;
            _documentDbOptions = documentDbOptions.Value;
        }

        public async Task RunAsync()
        {
            var facts = await GetLatestFactsAsync();
            using (var documentClient = _documentDbClientFactory.Create())
            {
                var databaseUri = UriFactory.CreateDatabaseUri("Nulobe");
                try
                {
                    await documentClient.DeleteDatabaseAsync(databaseUri);
                }
                catch (DocumentClientException ex)
                {
                    if (ex.Error.Code != "NotFound")
                    {
                        throw;
                    }
                }

                await documentClient.EnsureFactCollectionAsync();
                foreach (var fact in facts)
                {
                    await FrameworkDocumentClientExtensions.CreateFactDocumentAsync(documentClient, fact);
                }
            }
        }

        #region Helpers

        private async Task<dynamic[]> GetLatestFactsAsync()
        {
            var client = await _cloudBlobClientFactory.CreateBlobClient().GetCloudBlobContainerAsync("prodcopy");

            var dts = EnumerableSeedExtensions.Seed(10, prev => prev.AddDays(-1), DateTime.UtcNow);
            foreach (var dt in dts)
            {
                CloudBlob blob = null;
                try
                {
                    blob = await client.GetAsync(Path.Combine(new object[]
                    {
                        dt.Year,
                        dt.Month,
                        dt.Day,
                        "Nulobe.Facts.json"
                    }.Select(o => o.ToString()).ToArray()));
                }
                catch (StorageException ex)
                {
                    var innerWebException = ex.InnerException as WebException;
                    if (innerWebException != null)
                    {
                        var httpWebResponse = innerWebException.Response as HttpWebResponse;
                        if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.NotFound)
                        {
                            // A backup doesn't exist for this day, continue to the previous day.
                            continue;
                        }
                    }
                    throw;
                }

                using (var sr = new StreamReader(blob.Stream))
                {
                    return JsonConvert.DeserializeObject<dynamic[]>(await sr.ReadToEndAsync());
                }
            }

            throw new Exception("No data found within the last 10 days");
        }

        #endregion
    }
}
