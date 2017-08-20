using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using Nulobe.Utility.Query;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class FactQueryService : IFactQueryService
    {
        private readonly FactServiceOptions _factServiceOptions;
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly IDocumentClientFactory _documentClientFactory;

        public FactQueryService(
            IOptions<FactServiceOptions> factServiceOptions,
            IOptions<DocumentDbOptions> documentDbOptions,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IDocumentClientFactory documentClientFactory)
        {
            _factServiceOptions = factServiceOptions.Value;
            _documentDbOptions = documentDbOptions.Value;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _documentClientFactory = documentClientFactory;
        }

        public Task<FactQueryResult> QueryFactsAsync(FactQuery query)
        {
            var sqlQueryText = "SELECT ";

            var fieldPropertyNames = query.GetFieldPropertyNames();
            if (fieldPropertyNames.Any())
            {
                sqlQueryText += string.Join(" ", fieldPropertyNames.Select(f => $"f.{f}"));
            }
            else
            {
                sqlQueryText += "*";
            }

            sqlQueryText += " FROM Facts f";

            var tags = query.Tags
                .Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t));
            if (tags.Any())
            {
                sqlQueryText += " WHERE ";
                sqlQueryText += string.Join(" AND ", tags.Select((t, i) => $"ARRAY_CONTAINS(f.Tags, @tag{i})"));
            }

            var sqlParameters = new SqlParameterCollection(
                tags.Select((t, i) => new SqlParameter($"@tag{i}", t)));
            
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                var result = client.CreateDocumentQuery<Fact>(_documentDbOptions, _factServiceOptions.FactCollectionName, new SqlQuerySpec()
                {
                    QueryText = sqlQueryText,
                    Parameters = sqlParameters
                }).ToList();

                var pageNumber = query.GetPageNumber();
                var pageSize = query.GetPageSize();
                return Task.FromResult(new FactQueryResult()
                {
                    Count = result.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Facts = result
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .AsEnumerable()
                });
            }
        }
    }
}
