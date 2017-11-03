using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using Nulobe.DocumentDb.Client;
using Nulobe.Framework;
using Nulobe.Api.Core.Facts;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class NulobeSourceHandler : ISourceHandler
    {
        private readonly IDocumentClientFactory _documentClientFactory;

        public NulobeSourceHandler(
            IDocumentClientFactory documentClientFactory)
        {
            _documentClientFactory = documentClientFactory;
        }

        public SourceType Type => SourceType.Nulobe;

        public void PreValidate(JObject source, ModelErrorDictionary errors)
        {
            string factId = null;
            var factIdToken = source.SelectToken(SourceFields.FactId);
            if (factIdToken == null)
            {
                errors.AddRequired(SourceFields.FactId);
            }
            else if (factIdToken is JValue doiValue)
            {
                if (factIdToken.Type == JTokenType.String)
                {
                    factId = factIdToken.ToObject<string>();
                }
                else
                {
                    errors.AddStringExpected(SourceFields.FactId);
                }
            }
            else
            {
                errors.AddStringExpected(SourceFields.FactId);
            }
        }

        public async Task ProcessAsync(JObject source)
        {
            FactData fact = null;
            using (var client = _documentClientFactory.Create(readOnly: true))
            {
                try
                {
                    var factId = source.SelectToken(SourceFields.FactId).ToObject<string>();
                    fact = await client.ReadFactDocumentAsync<FactData>(factId);
                }
                catch (DocumentNotFoundException)
                {
                }
            }

            if (fact != null)
            {
                source.Remove(SourceFields.FactTitle);
                source.Add(SourceFields.FactTitle, fact.Title);
            }
        }

        public void PostValidate(JObject source, ModelErrorDictionary errors)
        {
            if (source.SelectToken(SourceFields.FactTitle) == null)
            {
                var factId = source.SelectToken(SourceFields.FactId).ToObject<string>();
                errors.Add($"Could not find fact with Id ${factId}", SourceFields.FactId);
            }
        }
    }
}
