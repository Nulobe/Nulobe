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

        public Task<IEnumerable<Tag>> QueryTagsAsync(TagQuery query)
        {
            using (var client = _documentClientFactory.Create(_factServiceOptions))
            {
                
            }
            return Task.FromResult(Enumerable.Empty<Tag>());
        }
    }
}
