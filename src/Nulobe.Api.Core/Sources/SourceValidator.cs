using MoreLinq;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public class SourceValidator
    {
        private readonly IEnumerable<ISourceValidationHandler> _handlers;

        public SourceValidator(
            IEnumerable<ISourceValidationHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task<SourceValidationResult> IsValidAsync(dynamic source)
        {
            if (!(source is JObject))
            {
                return SourceValidationResult.Invalid("Expected source to be a JSON object");
            }

            SourceValidationResult sourceTypeInvalid(string message) => SourceValidationResult.Invalid(message, "Type");
            SourceType sourceType = SourceType.Unknown;
            var sourceTypeInt = (int?)source.type;
            if (sourceTypeInt.HasValue)
            {
                sourceType = (SourceType)sourceTypeInt;
                if (!Enum.IsDefined(typeof(SourceType), sourceType) || sourceType == SourceType.Unknown)
                {
                    return sourceTypeInvalid("Source type value is invalid");
                }
            }
            else
            {
                return sourceTypeInvalid("Source type is required");
            }

            var handler = _handlers.FirstOrDefault(h => h.Type == sourceType);
            if (handler == null)
            {
                throw new Exception($"No {nameof(ISourceValidationHandler)} defined for {nameof(SourceType)}.{sourceType}");
            }

            var errors = new ModelErrorDictionary();

            SourceValidationResult handlerValidationResult = await handler.IsValidAsync(source);
            errors.Add(handlerValidationResult.ModelErrors);


            return await handler.IsValidAsync(source);
        }
    }
}
