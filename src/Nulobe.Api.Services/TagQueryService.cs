using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Nulobe.DocumentDb.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Services
{
    public class TagQueryService : ITagQueryService
    {
        private readonly FactServiceOptions _factServiceOptions;
        private readonly IDocumentClientFactory _documentClientFactory;

        public TagQueryService(
            IOptions<FactServiceOptions> factServiceOptions,
            IDocumentClientFactory documentClientFactory)
        {
            _factServiceOptions = factServiceOptions.Value;
            _documentClientFactory = documentClientFactory;
        }

        // TODO: Performance wont scale at all here, use a user-defined function to select tags inside of Cosmos, or just use a cache.
        public Task<IEnumerable<Tag>> QueryTagsAsync(TagQuery query)
        {
            var fields = query.Fields.Split(',');
            if (!fields.Any())
            {
                return Task.FromResult(Enumerable.Empty<Tag>());
            }

            var sqlQuery = new SqlQuerySpec()
            {
                QueryText = @"
                    SELECT f.Tags
                    FROM Facts f
                ",
                Parameters = new SqlParameterCollection()
                {
                    //new SqlParameter("@factcollectionname", _factServiceOptions.CollectionName)
                }
            };

            IEnumerable<Fact> facts = null;
            using (var client = _documentClientFactory.Create(_factServiceOptions))
            {
                facts = client.CreateDocumentQuery<Fact>(_factServiceOptions, sqlQuery).ToList();
            }

            var tags = facts
                .SelectMany(f => f.Tags)
                .GroupBy(t => t, StringComparer.InvariantCultureIgnoreCase)
                .Where(g =>
                    string.IsNullOrEmpty(query.SearchPattern) ||
                    g.Key.ToLowerInvariant().Contains(query.SearchPattern.ToLowerInvariant()))
                .Select(g =>
                {
                    var tag = new Tag();

                    if (fields.Contains(nameof(Tag.Text), StringComparer.OrdinalIgnoreCase))
                    {
                        tag.Text = g.Key;
                    }

                    if (fields.Contains(nameof(Tag.UsageCount), StringComparer.OrdinalIgnoreCase))
                    {
                        tag.UsageCount = g.Count();
                    }

                    return tag;
                });

            var orderBy = query.OrderBy;

            var directionSpecified = 0;
            if (orderBy.StartsWith("+"))
            {
                directionSpecified = 1;
            }
            else if (orderBy.StartsWith("-"))
            {
                directionSpecified = -1;
            }

            var isDirectionSpecified = directionSpecified != 0;
            var field = isDirectionSpecified ? orderBy.Skip(1).ToString() : orderBy;

            IOrderedEnumerable<Tag> orderedTags = null;
            if (directionSpecified < 0)
            {
                if (field.Equals(nameof(Tag.UsageCount)))
                {
                    orderedTags = tags.OrderByDescending(t => t.UsageCount);
                }
                else
                {
                    orderedTags = tags.OrderByDescending(t => t.Text);
                }
            }
            else
            {
                if (field.Equals(nameof(Tag.UsageCount)))
                {
                    orderedTags = tags.OrderBy(t => t.UsageCount);
                }
                else
                {
                    orderedTags = tags.OrderBy(t => t.Text);
                }
            }

            return Task.FromResult(orderedTags.AsEnumerable());
        }
    }
}
