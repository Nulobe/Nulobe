using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Nulobe.Api.Core.Facts;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
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

        private readonly DocumentDbOptions _documentDbOptions;
        private readonly FactServiceOptions _factServiceOptions;
        private readonly IDocumentClientFactory _documentClientFactory;

        public TagMemoryRepository(
            IOptions<DocumentDbOptions> documentDbOptions,
            IOptions<FactServiceOptions> factServiceOptions,
            IDocumentClientFactory documentClientFactory)
        {
            _documentDbOptions = documentDbOptions.Value;
            _factServiceOptions = factServiceOptions.Value;
            _documentClientFactory = documentClientFactory;
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
                        _cachedTags = await GetTagsFromDatabaseAsync();
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            return _cachedTags;
        }

        private Task<IEnumerable<Tag>> GetTagsFromDatabaseAsync()
        {
            var sqlQuery = new SqlQuerySpec()
            {
                QueryText = @"
                    SELECT f.Tags
                    FROM Facts f"
            };

            IEnumerable<Fact> facts = null;
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                facts = client.CreateDocumentQuery<Fact>(_documentDbOptions, Constants.FactCollectionName, sqlQuery).ToList();
            }

            var tags = facts
                .SelectMany(f => f.Tags)
                .GroupBy(t => t, StringComparer.InvariantCultureIgnoreCase)
                .Select(g => new Tag()
                {
                    Text = g.Key,
                    UsageCount = g.Count()
                });

            return Task.FromResult(tags);
        }
    }
}
