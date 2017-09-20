using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using MoreLinq;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using Nulobe.Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Jobs.TagIndexingJob
{
    public class TagIndexingJob
    {
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;

        public TagIndexingJob(
            IDocumentClientFactory documentClientFactory,
            ICloudStorageClientFactory cloudStorageClientFactory)
        {
            _documentClientFactory = documentClientFactory;
            _cloudStorageClientFactory = cloudStorageClientFactory;
        }

        public async Task RunAsync()
        {
            var sqlQuery = new SqlQuerySpec()
            {
                QueryText = @"
                    SELECT f.Tags
                    FROM Facts f"
            };

            IEnumerable<Fact> facts = null;
            using (var client = _documentClientFactory.Create(readOnly: true))
            {
                facts = client.CreateFactDocumentQuery<Fact>(new SqlQuerySpec()
                {
                    QueryText = @"
                    SELECT f.Tags
                    FROM Facts f"
                }).ToList();
            }

            var tags = facts
               .SelectMany(f => f.Tags)
               .GroupBy(t => t, StringComparer.InvariantCultureIgnoreCase)
               .Select(g => new Tag(g.Key) { UsageCount = g.Count() });

            var tableClient = _cloudStorageClientFactory.CreateTableClient();
            var tagTableRef = tableClient.GetTagsTableReference();

            await tagTableRef.CreateIfNotExistsAsync();

            var tagsToRemove = await GetRemovedTagsAsync(tagTableRef, tags);
            await tagTableRef.DeleteBatchAsync(tagsToRemove);

            await tagTableRef.InsertOrReplaceBatchAsync(tags);
        }

        private async Task<IEnumerable<Tag>> GetRemovedTagsAsync(CloudTable tagTableRef, IEnumerable<Tag> newTags)
        {
            var existingTags = await tagTableRef.ListAsync<Tag>();
            return existingTags.ExceptBy(newTags, t => t.Text, StringComparer.InvariantCultureIgnoreCase);
        }

        private class Fact
        {
            public IEnumerable<string> Tags { get; set; }
        }

        private class Tag : TableEntity
        {
            public Tag()
            {
            }

            public Tag(string text) : base("NOT-PARTITIONED", text.ToUpperInvariant())
            {
                Text = text;
            }

            public string Text { get; set; }
            public int UsageCount { get; set; }
        }
    }
}
