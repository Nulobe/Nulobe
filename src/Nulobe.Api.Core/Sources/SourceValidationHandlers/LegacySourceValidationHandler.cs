using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.SourceValidationHandlers
{
    public class LegacySourceValidationHandler : ISourceValidationHandler
    {
        public SourceType Type => SourceType.Legacy;

        public IEnumerable<string> Fields => throw new NotImplementedException();

        public Task<SourceValidationResult> IsValidAsync(dynamic source)
        {
            string sourceUrl = source.url;
            if (string.IsNullOrEmpty(sourceUrl))
            {
                return Task.FromResult(SourceValidationResult.Invalid("The field Url is required", "Url"));
            }

            if (!ValidationUtility.IsValidUri(sourceUrl))
            {
                return Task.FromResult(SourceValidationResult.Invalid("The field Url is not valid", "Url"));
            }

            return Task.FromResult(SourceValidationResult.Valid());
        }
    }
}
