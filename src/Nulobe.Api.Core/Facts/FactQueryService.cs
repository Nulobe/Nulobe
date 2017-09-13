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
using Microsoft.Azure.Documents.Client;
using AutoMapper;
using Microsoft.Azure.Documents.Linq;

namespace Nulobe.Api.Core.Facts
{
    public class FactQueryService : IFactQueryService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FactServiceOptions _factServiceOptions;
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly IMapper _mapper;

        public FactQueryService(
            IServiceProvider serviceProvider,
            IOptions<FactServiceOptions> factServiceOptions,
            IOptions<DocumentDbOptions> documentDbOptions,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IDocumentClientFactory documentClientFactory,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _factServiceOptions = factServiceOptions.Value;
            _documentDbOptions = documentDbOptions.Value;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _documentClientFactory = documentClientFactory;
            _mapper = mapper;
        }

        public async Task<FactQueryResult> QueryFactsAsync(FactQuery query)
        {
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                SqlQuerySpec querySpec = null;
                if (!string.IsNullOrEmpty(query.Tags))
                {
                    querySpec = GetTagFactQuerySpec(query.Tags, query.GetFieldPropertyNames());
                }
                else if (!string.IsNullOrEmpty(query.Slug))
                {
                    querySpec = GetSlugFactQuerySpec(query.Slug, query.GetFieldPropertyNames());
                }
                else
                {
                    querySpec = GetBaseFactQuerySpec(query.GetFieldPropertyNames());
                }

                var feedOptions = new FeedOptions()
                {
                    MaxItemCount = query.PageSize
                };

                var documentQuery = client.CreateDocumentQuery<FactData>(
                    _documentDbOptions,
                    _factServiceOptions.FactCollectionName,
                    querySpec,
                    feedOptions).AsDocumentQuery();

                var results = await documentQuery.ExecuteNextAsync<FactData>();
                return new FactQueryResult()
                {
                    Facts = results.Select(f => _mapper.MapWithServices<FactData, Fact>(f, _serviceProvider)),
                    ContinuationToken = results.ResponseContinuation
                };
            }
        }


        #region Helpers

        private SqlQuerySpec GetTagFactQuerySpec(string tagsStr, IEnumerable<string> fieldPropertyNames)
        {
            var sqlQueryText = GetBaseSqlQuery(fieldPropertyNames);

            var tags = tagsStr
                .Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t));

            if (tags.Any())
            {
                sqlQueryText += " WHERE ";
                sqlQueryText += string.Join(" AND ", tags.Select((t, i) => $"ARRAY_CONTAINS(f.Tags, @tag{i})"));
            }

            var sqlParameters = new SqlParameterCollection(tags.Select((t, i) => new SqlParameter($"@tag{i}", t)));

            return new SqlQuerySpec()
            {
                QueryText = sqlQueryText,
                Parameters = sqlParameters
            };
        }

        private SqlQuerySpec GetSlugFactQuerySpec(string slug, IEnumerable<string> fieldPropertyNames)
        {
            var sqlQueryText = GetBaseSqlQuery(fieldPropertyNames) + " WHERE f.Slug = @slug";
            var sqlParameters = new SqlParameterCollection(new SqlParameter[] { new SqlParameter("@slug", slug) });
            return new SqlQuerySpec()
            {
                QueryText = sqlQueryText,
                Parameters = sqlParameters
            };
        }

        private SqlQuerySpec GetBaseFactQuerySpec(IEnumerable<string> fieldPropertyNames)
        {
            return new SqlQuerySpec()
            {
                QueryText = GetBaseSqlQuery(fieldPropertyNames),
                Parameters = new SqlParameterCollection()
            };
        }

        private string GetBaseSqlQuery(IEnumerable<string> fieldPropertyNames)
        {
            var sqlQueryText = "SELECT ";
            
            if (fieldPropertyNames.Any())
            {
                sqlQueryText += string.Join(" ", fieldPropertyNames.Select(f => $"f.{f}"));
            }
            else
            {
                sqlQueryText += "*";
            }

            sqlQueryText += " FROM Facts f";

            return sqlQueryText;
        }

        #endregion
    }
}
