using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nulobe.DocumentDb.Client;
using AutoMapper;
using Nulobe.Framework;
using Nulobe.Utility.Validation;
using System.Text.RegularExpressions;
using Nulobe.System.Collections.Generic;
using Nulobe.Utility.SlugBuilder;

namespace Nulobe.Api.Core.Facts
{
    public class FactService : IFactService
    {
        private static readonly Regex SourceReferenceRegex = new Regex(@"\[(\d+)\]");

        private readonly IServiceProvider _serviceProvider;
        private readonly FactServiceOptions _factServiceOptions;
        private readonly DocumentDbOptions _documentDbOptions;
        private readonly CountryOptions _countryOptions;
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly Auditor _auditor;
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly IMapper _mapper;

        public FactService(
            IServiceProvider serviceProvider,
            IOptions<FactServiceOptions> factServiceOptions,
            IOptions<DocumentDbOptions> documentDbOptions,
            IOptions<CountryOptions> countryOptions,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            Auditor auditor,
            IDocumentClientFactory documentClientFactory,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _factServiceOptions = factServiceOptions.Value;
            _documentDbOptions = documentDbOptions.Value;
            _countryOptions = countryOptions.Value;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _auditor = auditor;
            _documentClientFactory = documentClientFactory;
            _mapper = mapper;
        }

        public async Task<Fact> GetFactAsync(string id)
        {
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                try
                {
                    var fact = await client.ReadDocumentAsync<FactData>(_documentDbOptions, _factServiceOptions.FactCollectionName, id);
                    return _mapper.MapWithServices<FactData, Fact>(fact, _serviceProvider);
                }
                catch (DocumentNotFoundException ex)
                {
                    throw new ClientEntityNotFoundException(typeof(Fact), id, ex);
                }
            }
        }

        public async Task<Fact> CreateFactAsync(FactCreate create)
        {
            AssertAuthenticated();
            Validator.ValidateNotNull(create, nameof(create));
            ValidateFactCreate(create);

            var fact = _mapper.Map<FactData>(create);
            var factAudit = new FactAudit() { CurrentValue = fact };
            _auditor.AuditAction(nameof(CreateFactAsync), factAudit);

            fact.Slug = GenerateSlug(fact);
            fact.SlugHistory = new FactDataSlugAudit[]
            {
                new FactDataSlugAudit()
                {
                    Slug = fact.Slug,
                    Created = factAudit.Actioned
                }
            };

            fact.Id = Guid.NewGuid().ToString();
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                await client.CreateDocumentAsync(_documentDbOptions, _factServiceOptions.FactCollectionName, fact);
                await client.CreateDocumentAsync(_documentDbOptions, _factServiceOptions.FactAuditCollectionName, factAudit);
            }

            return _mapper.MapWithServices<FactData, Fact>(fact, _serviceProvider);
        }

        public async Task<Fact> UpdateFactAsync(string id, FactCreate create)
        {
            AssertAuthenticated();
            Validator.ValidateNotNull(create, nameof(create));
            ValidateFactCreate(create);

            var fact = _mapper.Map<FactData>(create);
            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                var existingFact = await client.ReadDocumentAsync<FactData>(_documentDbOptions, _factServiceOptions.FactCollectionName, id);
                if (existingFact == null)
                {
                    throw new ClientEntityNotFoundException(typeof(FactData), id);
                }
                
                fact.Id = id;
                var factAudit = new FactAudit()
                {
                    CurrentValue = fact,
                    PreviousValue = existingFact
                };
                _auditor.AuditAction(nameof(UpdateFactAsync), factAudit);

                if (fact.Title.Equals(existingFact.Title, StringComparison.OrdinalIgnoreCase))
                {
                    fact.Slug = existingFact.Slug;
                    fact.SlugHistory = existingFact.SlugHistory;
                }
                else
                {
                    fact.Slug = GenerateSlug(fact);
                    fact.SlugHistory = Enumerable.Empty<FactDataSlugAudit>()
                        .Concat(existingFact.SlugHistory)
                        .Concat(new FactDataSlugAudit[]
                        {
                            new FactDataSlugAudit()
                            {
                                Slug = fact.Slug,
                                Created = factAudit.Actioned
                            }
                        });
                }

                await client.CreateDocumentAsync(_documentDbOptions, _factServiceOptions.FactAuditCollectionName, factAudit);
                await client.ReplaceDocumentAsync(_documentDbOptions, _factServiceOptions.FactCollectionName, id, create);
            }

            return _mapper.MapWithServices<FactData, Fact>(fact, _serviceProvider);
        }

        public async Task DeleteFactAsync(string id)
        {
            AssertAuthenticated();
            Validator.ValidateStringNotNullOrEmpty(id, nameof(id));

            using (var client = _documentClientFactory.Create(_documentDbOptions))
            {
                var fact = await client.ReadDocumentAsync<FactData>(_documentDbOptions, _factServiceOptions.FactCollectionName, id);
                if (fact == null)
                {
                    throw new ClientEntityNotFoundException(typeof(Fact), id);
                }

                var factAudit = new FactAudit() { PreviousValue = fact };
                _auditor.AuditAction(nameof(DeleteFactAsync), factAudit);

                await client.CreateDocumentAsync(_documentDbOptions, _factServiceOptions.FactAuditCollectionName, factAudit);
                await client.DeleteDocumentAsync(_documentDbOptions, _factServiceOptions.FactCollectionName, id);
            }
        }


        #region Helpers

        private void AssertAuthenticated()
        {
            if (!_claimsPrincipalAccessor.ClaimsPrincipal.Identity.IsAuthenticated)
            {
                throw new ClientUnauthenticatedException();
            }
        }

        private void ValidateFactCreate(FactCreate create)
        {
            var (isValid, modelErrors) = Validator.IsValid(create);

            if (modelErrors.IsMemberValid(nameof(FactCreate.Definition)))
            {
                var indexSequence = SourceReferenceRegex.Matches(create.Definition)
                    .Cast<Match>()
                    .Select(m =>
                    {
                        var groups = m.Groups.Cast<Group>();
                        var group = groups.Skip(1).First();

                        int.TryParse(group.Value, out int index);
                        return index;
                    })
                    .Where(i => i >= 1 && i <= _factServiceOptions.MaxSourceCount);

                if (indexSequence.Any())
                {
                    if (indexSequence.Max() != create.Sources.Count())
                    {
                        modelErrors.Add(
                            $"Expected number of sources ({create.Sources.Count()}) to equal the number referenced in {nameof(FactCreate.Definition)} ({indexSequence.Max()})",
                            nameof(FactCreate.Definition));
                    }

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

            if (!string.IsNullOrEmpty(create.Country) && !_countryOptions.ContainsKey(create.Country))
            {
                modelErrors.Add($"{create.Country} is not an available country", nameof(create.Country));
            }

            if (modelErrors.Errors.Count() > 0)
            {
                throw new ClientModelValidationException(modelErrors);
            }
        }

        private string GenerateSlug(FactData fact)
        {
            var slugBuilder = new SlugBuilderFactory().Create();
            slugBuilder.Add(new Random().Next(10000, 99999));
            slugBuilder.Add(fact.Title);
            return slugBuilder.ToString();
        }

        #endregion

    }
}
