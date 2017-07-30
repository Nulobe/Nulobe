using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeganFacts.DocumentDb.Client;

namespace VeganFacts.Services
{
    public class FactService : IFactService, IFactQueryService
    {
        private readonly FactServiceOptions _factServiceOptions;
        private readonly IDocumentClientFactory _documentClientFactory;

        public FactService(
            IOptions<FactServiceOptions> factServiceOptions,
            IDocumentClientFactory documentClientFactory)
        {
            _factServiceOptions = factServiceOptions.Value;
            _documentClientFactory = documentClientFactory;
        }

        public Task<Fact> GetFactAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Fact> CreateFactAsync(Fact fact)
        {
            fact.Id = Guid.NewGuid().ToString();
            using (var client = _documentClientFactory.Create(_factServiceOptions))
            {
                await client.CreateDocumentAsync(_factServiceOptions, fact);
            }
            return fact;
        }

        public Task<Fact> DeleteFactAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Fact> UpdateFactAsync(string id, Fact fact)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Fact>> QueryFactsAsync(FactQuery query)
        {
            await Task.FromResult(0);
            using (var client = _documentClientFactory.Create(_factServiceOptions))
            {
                return client.CreateDocumentQuery<Fact>(_factServiceOptions)
                    .ToList();
            }
        }

    }
}
