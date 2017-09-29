using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Tools.BlobStorageImport
{
    public class BlobStorageImportService
    {
        private readonly IDocumentClientFactory _documentDbClientFactory;
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly BlobStorageImportOptions _blobStorageImportOptions;

        public BlobStorageImportService(
            IDocumentClientFactory documentDbClientFactory,
            IOptions<DocumentDbOptions> documentDbOptions,
            IOptions<BlobStorageImportOptions> blobStorageImportOptions)
        {
            _documentDbClientFactory = documentDbClientFactory;
            _documentDbOptions = documentDbOptions.Value;
            _blobStorageImportOptions = blobStorageImportOptions.Value;
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
            var dts = EnumerableSeedExtensions.Seed(10, prev => prev.AddDays(-1), DateTime.UtcNow.AddDays(-1));
            using (var client = new HttpClient())
            {
                foreach (var dt in dts)
                {
                    var response = await client.GetAsync(Path.Combine(new object[]
                    {
                        _blobStorageImportOptions.ContainerUrl,
                        dt.Year,
                        dt.Month,
                        dt.Day,
                        "Nulobe.Facts.json"
                    }.Select(o => o.ToString()).ToArray()));

                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        response.EnsureSuccessStatusCode();

                        using (var sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        {
                            return JsonConvert.DeserializeObject<dynamic[]>(await sr.ReadToEndAsync());
                        }
                    }
                }
            }

            throw new Exception("No data found within the last 10 days");
        }

        #endregion
    }
}
