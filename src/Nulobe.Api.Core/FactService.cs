using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nulobe.DocumentDb.Client;
using Microsoft.Azure.Documents;

namespace Nulobe.Api.Core
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

        public Task<IEnumerable<Fact>> QueryFactsAsync(FactQuery query)
        {
            var tags = query.Tags
                .Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t));

            var sqlQueryText = "SELECT * FROM Facts f";
            if (tags.Any())
            {
                sqlQueryText += " WHERE ";
                sqlQueryText += string.Join(" OR ", tags.Select((t, i) => $"ARRAY_CONTAINS(f.Tags, @tag{i})"));
            }

            var sqlParameters = new SqlParameterCollection(
                tags.Select((t, i) => new SqlParameter($"@tag{i}", t)));

            using (var client = _documentClientFactory.Create(_factServiceOptions))
            {
                var result = client.CreateDocumentQuery<Fact>(_factServiceOptions, new SqlQuerySpec()
                {
                    QueryText = sqlQueryText,
                    Parameters = sqlParameters
                }).ToList();

                return Task.FromResult(result.AsEnumerable());
            }
        }

    }
}
