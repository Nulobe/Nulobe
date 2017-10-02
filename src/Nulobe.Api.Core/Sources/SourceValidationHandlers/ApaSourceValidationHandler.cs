using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.SourceValidationHandlers
{
    public class ApaSourceValidationHandler : ISourceValidationHandler
    {
        public SourceType Type => SourceType.Apa;

        public IEnumerable<string> Fields => new string[] { "apaType", "authors" };

        public Task<SourceValidationResult> IsValidAsync(dynamic source)
        {
            var errors = new ModelErrorDictionary();

            void apaSourceTypeInvalid(string message) => errors.Add("ApaType", message);

            ApaSourceType apaSourceType = ApaSourceType.Unknown;
            var apaSourceTypeInt = (int?)source.apaType;
            if (apaSourceTypeInt.HasValue)
            {
                apaSourceType = (ApaSourceType)apaSourceTypeInt;
                if (!Enum.IsDefined(typeof(ApaSourceType), apaSourceType) || apaSourceType == ApaSourceType.Unknown)
                {
                    apaSourceTypeInvalid("APA source type value is invalid");
                }
            }
            else
            {
                apaSourceTypeInvalid("APA source type is required");
            }

            var authors = source.authors as JArray;
            for (var i = 0; i < authors.Count(); i++)
            {
                var author = authors[i];
                if (string.IsNullOrEmpty(author.ToString()))
                {
                    errors.Add($"Author must not a non-empty string", $"[{i}]");
                }
            }

            return Task.FromResult(errors.Any() ? SourceValidationResult.Invalid(errors) : SourceValidationResult.Valid());
        }
    }
}
