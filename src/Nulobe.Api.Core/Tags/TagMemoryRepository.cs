using Microsoft.WindowsAzure.Storage.Table;
using Nulobe.Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Tags
{
    public class TagMemoryRepository : ITagMemoryRepository
    {
        private static readonly TimeSpan MaxTagsAge = TimeSpan.FromMinutes(5);

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private IEnumerable<Tag> _cachedTags = Enumerable.Empty<Tag>();
        private DateTime _cachedTagsAge = DateTime.MinValue;
        
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;

        public TagMemoryRepository(
            ICloudStorageClientFactory cloudStorageClientFactory)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            if (DateTime.UtcNow > _cachedTagsAge + MaxTagsAge)
            {
                try
                {
                    await _lock.WaitAsync();

                    if (DateTime.UtcNow > _cachedTagsAge + MaxTagsAge)
                    {
                        _cachedTags = await GetTagsFromTableAsync();
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            return _cachedTags;
        }

        private async Task<IEnumerable<Tag>> GetTagsFromTableAsync()
        {
            var tableClient = _cloudStorageClientFactory.CreateTableClient();
            var tagTableRef = tableClient.GetTagsTableReference();

            await tagTableRef.CreateIfNotExistsAsync();

            var tagEntities = await tagTableRef.ListAsync<TagEntity>();
            return tagEntities.Select(e => new Tag
            {
                Text = e.Text,
                UsageCount = e.UsageCount
            });
        }

        private class TagEntity : TableEntity
        {
            public TagEntity()
            {
            }

            public TagEntity(string text) : base("NOT-PARTITIONED", text.ToUpperInvariant())
            {
                Text = text;
            }

            public string Text { get; set; }
            public int UsageCount { get; set; }
        }
    }
}
