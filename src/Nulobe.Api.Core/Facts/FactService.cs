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
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Nulobe.Microsoft.WindowsAzure.Storage;
using Nulobe.Api.Core.Sources;

namespace Nulobe.Api.Core.Facts
{
    public class FactService : IFactService
    {
        private static readonly Regex SourceReferenceRegex = new Regex(@"\[(\d+)\]");

        private readonly IServiceProvider _serviceProvider;
        private readonly FactServiceOptions _factServiceOptions;
        private readonly CountryOptions _countryOptions;
        private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
        private readonly Auditor _auditor;
        private readonly SourceValidator _sourceValidator;
        private readonly IDocumentClientFactory _documentClientFactory;
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public FactService(
            IServiceProvider serviceProvider,
            IOptions<FactServiceOptions> factServiceOptions,
            IOptions<CountryOptions> countryOptions,
            IClaimsPrincipalAccessor claimsPrincipalAccessor,
            Auditor auditor,
            SourceValidator sourceValidator,
            IDocumentClientFactory documentClientFactory,
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _factServiceOptions = factServiceOptions.Value;
            _countryOptions = countryOptions.Value;
            _claimsPrincipalAccessor = claimsPrincipalAccessor;
            _auditor = auditor;
            _sourceValidator = sourceValidator;
            _documentClientFactory = documentClientFactory;
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        public async Task<Fact> GetFactAsync(string id)
        {
            using (var client = _documentClientFactory.Create(readOnly: true))
            {
                try
                {
                    var fact = await client.ReadFactDocumentAsync<FactData>(id);
                    return _mapper.MapWithServices<FactData, Fact>(fact, _serviceProvider);
                }
                catch (DocumentNotFoundException ex)
                {
                    throw new ClientEntityNotFoundException(typeof(Fact), id, ex);
                }
            }
        }

        public async Task<Fact> CreateFactAsync(FactCreate create, bool dryRun = false)
        {
            AssertAuthenticated();
            Validator.ValidateNotNull(create, nameof(create));
            await ValidateFactCreateAsync(create);

            var fact = _mapper.Map<FactData>(create);
            fact.Id = Guid.NewGuid().ToString();
            fact.Slug = GenerateSlug(fact);
            fact.SlugHistory = new FactDataSlugAudit[]
            {
                new FactDataSlugAudit()
                {
                    Slug = fact.Slug,
                    Created = DateTime.UtcNow
                }
            };

            if (!dryRun)
            {
                await AuditAsync(nameof(CreateFactAsync), fact);
                using (var client = _documentClientFactory.Create())
                {
                    await client.CreateFactDocumentAsync(fact);
                }
            }

            return _mapper.MapWithServices<FactData, Fact>(fact, _serviceProvider);
        }

        public async Task<Fact> UpdateFactAsync(string id, FactCreate create)
        {
            AssertAuthenticated();
            Validator.ValidateNotNull(create, nameof(create));
            await ValidateFactCreateAsync(create);

            var fact = _mapper.Map<FactData>(create);
            using (var client = _documentClientFactory.Create())
            {
                var existingFact = await client.ReadFactDocumentAsync<FactData>(id);
                if (existingFact == null)
                {
                    throw new ClientEntityNotFoundException(typeof(FactData), id);
                }
                fact.Id = existingFact.Id;

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
                                Created = DateTime.UtcNow
                            }
                        });
                }

                await AuditAsync(nameof(UpdateFactAsync), fact, existingFact);
                await client.ExecuteEnsuredStoredProcedureAsync<FactData>("Nulobe", "Facts", "_db/StoredProcedures/updateFact.js", new dynamic[] { fact });
            }

            return _mapper.MapWithServices<FactData, Fact>(fact, _serviceProvider);
        }

        public async Task DeleteFactAsync(string id)
        {
            AssertAuthenticated();
            Validator.ValidateStringNotNullOrEmpty(id, nameof(id));

            using (var client = _documentClientFactory.Create())
            {
                var fact = await client.ReadFactDocumentAsync<FactData>(id);
                if (fact == null)
                {
                    throw new ClientEntityNotFoundException(typeof(Fact), id);
                }

                await AuditAsync(nameof(UpdateFactAsync), null, fact);
                await client.DeleteFactDocumentAsync(id);
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

        private async Task ValidateFactCreateAsync(FactCreate create)
        {
            var (isValid, modelErrors) = Validator.IsValid(create);

            if (modelErrors.IsMemberValid(nameof(FactCreate.DefinitionMarkdown)))
            {
                var indexSequence = SourceReferenceRegex.Matches(create.DefinitionMarkdown)
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
                            $"Expected number of sources ({create.Sources.Count()}) to equal the number referenced in {nameof(FactCreate.DefinitionMarkdown)} ({indexSequence.Max()})",
                            nameof(FactCreate.DefinitionMarkdown));
                    }

                    var expectedIndexSequence = ListHelpers.Range(1, indexSequence.Max());
                    if (!expectedIndexSequence.SequenceEqual(indexSequence))
                    {
                        modelErrors.Add(
                            $"Source references in {nameof(FactCreate.DefinitionMarkdown)} should be ascending and without missing indicies. Index sequence was [{string.Join(", ", indexSequence)}]",
                            nameof(FactCreate.DefinitionMarkdown));
                    }
                }
            }

            var sources = create.Sources.ToList();
            for (var i = 0; i < sources.Count(); i++)
            {
                SourceValidationResult validationResult = await _sourceValidator.IsValidAsync(sources[i]);
                if (!validationResult.IsValid)
                {
                    modelErrors.Add(validationResult.ModelErrors, $"{nameof(create.Sources)}[{i}]");
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

        private async Task AuditAsync(string actionName, FactData currentValue = null, FactData previousValue = null)
        {
            string getJson(FactData data) => data == null ? null : JsonConvert.SerializeObject(data);

            var factAudit = new FactAudit()
            {
                PartitionKey = currentValue?.Id ?? previousValue?.Id,
                RowKey = Guid.NewGuid().ToString(),
                CurrentValueJson = getJson(currentValue),
                PreviousValueJson = getJson(previousValue)
            };

            _auditor.AuditAction(actionName, factAudit);

            var tableClient = _cloudStorageClientFactory.CreateTableClient();
            var table = tableClient.GetTableReference("factaudits");
            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Insert(factAudit));
        }

        #endregion

    }
}
