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
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                IEnumerable<Fact> result = null;
                if (!string.IsNullOrEmpty(query.Tags))
                {
                    result = GetTagFactQueryable(client, query.Tags, query.GetFieldPropertyNames());
                }
                else if (!string.IsNullOrEmpty(query.Slug))
                {
                    result = GetSlugFactQueryable(client, query.Slug, query.GetFieldPropertyNames());
                }
                else
                {
                    result = GetBaseFactQueryable(client, query.GetFieldPropertyNames());
                }

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


        #region Helpers

        private IEnumerable<Fact> GetTagFactQueryable(DocumentClient client, string tagsStr, IEnumerable<string> fieldPropertyNames)
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

            return GetFactQueryable(client, sqlQueryText, sqlParameters);
        }

        private IEnumerable<Fact> GetSlugFactQueryable(DocumentClient client, string slug, IEnumerable<string> fieldPropertyNames)
        {
            var sqlQueryText = GetBaseSqlQuery(fieldPropertyNames) + " WHERE f.Slug = @slug";
            var sqlParameters = new SqlParameterCollection(new SqlParameter[] { new SqlParameter("@slug", slug) });
            return GetFactQueryable(client, sqlQueryText, sqlParameters);
        }

        private IEnumerable<Fact> GetBaseFactQueryable(DocumentClient client, IEnumerable<string> fieldPropertNames)
        {
            return GetFactQueryable(client, GetBaseSqlQuery(fieldPropertNames));
        }

        private IEnumerable<Fact> GetFactQueryable(DocumentClient client, string sqlQueryText, SqlParameterCollection sqlParameters = null)
        {
            return client.CreateDocumentQuery<Fact>(_documentDbOptions, _factServiceOptions.FactCollectionName, new SqlQuerySpec()
            {
                QueryText = sqlQueryText,
                Parameters = sqlParameters ?? new SqlParameterCollection()
            }).ToList();
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
