using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nulobe.DocumentDb.Client;
using Microsoft.Azure.Documents;
using AutoMapper;
using Nulobe.Framework;
using Nulobe.Utility.Validation;
using System.Text.RegularExpressions;
using Nulobe.System.Collections.Generic;

namespace Nulobe.Api.Core.Facts
{
    public class FactService : IFactService, IFactQueryService
    {
        private static readonly Regex SourceReferenceRegex = new Regex(@"\[(\d+)\]");

        private readonly FactServiceOptions _factServiceOptions;
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly IMapper _mapper;

        public FactService(
            IOptions<FactServiceOptions> factServiceOptions,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            IRemoteIpAddressAccessor remoteIpAddressAccessor,
            IDocumentClientFactory documentClientFactory,
            IMapper mapper)
        {
            _factServiceOptions = factServiceOptions.Value;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _documentClientFactory = documentClientFactory;
            _mapper = mapper;
        }

        public Task<Fact> GetFactAsync(string id)
        {
            using (var client = _documentClientFactory.Create(_factServiceOptions))
            {
                return client.ReadDocumentAsync<Fact>(_factServiceOptions, id);
            }
        }

        public async Task<Fact> CreateFactAsync(FactCreate create)
        {
            var principal = _claimsPrincipalAccessor.ClaimsPrincipal;
            if (!principal.Identity.IsAuthenticated)
            {
                throw new ClientUnauthenticatedException();
            }

            Validator.ValidateNotNull(create, nameof(create));
            var (isValid, modelErrors) = Validator.IsValid(create);

            if (modelErrors.IsMemberValid(nameof(FactCreate.Definition)))
            {
                var indexSequence = SourceReferenceRegex.Matches(create.Definition)
                    .Cast<Match>()
                    .Select(m =>
                    {
                        var groups = m.Groups.Cast<Group>();
                        var group = groups.Skip(1).First();

                        int index = 0;
                        int.TryParse(group.Value, out index);
                        return index;
                    })
                    .Where(i => i >= 1 && i <= _factServiceOptions.MaxSourceCount);

                if (indexSequence.Max() != create.Sources.Count())
                {
                    modelErrors.Add(
                        $"Expected number of sources ({create.Sources.Count()}) to equal the number referenced in {nameof(FactCreate.Definition)} ({indexSequence.Max()})",
                        nameof(FactCreate.Definition));
                }

                if (indexSequence.Any())
                {
                    var expectedIndexSequence = ListHelpers.Range(1, indexSequence.Max());
                    if (!expectedIndexSequence.SequenceEqual(indexSequence))
                    {
                        modelErrors.Add(
                            $"Source references in {nameof(FactCreate.Definition)} should be ascending and without missing indicies. Index sequence was [{string.Join(", ", indexSequence)}]",
                            nameof(FactCreate.Definition));
                    }
                }
            }

            var sources = create.Sources.ToList();
            for (var i = 0; i < sources.Count(); i++)
            {
                var (isSourceValid, sourceModelErrors) = Validator.IsValid(sources[i]);
                if (!isSourceValid)
                {
                    modelErrors.Add(sourceModelErrors, $"{nameof(create.Sources)}[{i}]");
                }
            }

            if (modelErrors.Errors.Count() > 0)
            {
                throw new ClientModelValidationException(modelErrors);
            }

            var fact = _mapper.Map<FactCreate, Fact>(create);
            fact.Created = DateTime.UtcNow;
            fact.CreatedById = principal.Identities.First().GetId();
            fact.CreatedByRemoteIp = _remoteIpAddressAccessor.RemoteIpAddress;

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
