using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Nulobe.Api.Core.Facts;
using Nulobe.DocumentDb.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Tags
{
    public class TagQueryService : ITagQueryService
    {
        private readonly ITagMemoryRepository _tagMemoryRepository;

        public TagQueryService(
            ITagMemoryRepository tagMemoryRepository)
        {
            _tagMemoryRepository = tagMemoryRepository;
        }
        
        public async Task<IEnumerable<Tag>> QueryTagsAsync(TagQuery query)
        {
            var fields = query.Fields.Split(',');
            if (!fields.Any())
            {
                return Enumerable.Empty<Tag>();
            }
            
            var tags = (await _tagMemoryRepository.GetTagsAsync())
                .Where(t =>
                    string.IsNullOrEmpty(query.SearchPattern) ||
                    t.Text.ToLowerInvariant().Contains(query.SearchPattern.ToLowerInvariant()))
                .Select(t =>
                {
                    var tag = new Tag();

                    if (fields.Contains(nameof(Tag.Text), StringComparer.OrdinalIgnoreCase))
                    {
                        tag.Text = t.Text;
                    }

                    if (fields.Contains(nameof(Tag.UsageCount), StringComparer.OrdinalIgnoreCase))
                    {
                        tag.UsageCount = t.UsageCount;
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

            return orderedTags.AsEnumerable();
        }
    }
}
