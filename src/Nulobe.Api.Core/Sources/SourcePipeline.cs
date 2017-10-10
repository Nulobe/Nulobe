using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public class SourcePipeline
    {
        private readonly IEnumerable<ISourceHandler> _handlers;

        public SourcePipeline(
            IEnumerable<ISourceHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task RunAsync(JObject source, ModelErrorDictionary errors)
        {
            // TODO: Validate properties here.

            var sourceType = GetValidSourceType(source, errors);
            if (errors.HasErrors)
            {
                return;
            }

            var handler = _handlers.FirstOrDefault(h => h.Type == sourceType);
            if (handler == null)
            {
                throw new NotImplementedException($"No {nameof(ISourceHandler)} implemented for {nameof(SourceType)}.{sourceType}");
            }

            handler.PreValidate(source, errors);
            if (errors.HasErrors)
            {
                return;
            }

            await handler.ProcessAsync(source);

            handler.PostValidate(source, errors);
        }


        private SourceType GetValidSourceType(JObject source, ModelErrorDictionary errors)
        {
            SourceType sourceType = SourceType.Unknown;

            var sourceTypeToken = source.SelectToken(SourceFields.Type);
            if (sourceTypeToken == null)
            {
                errors.AddRequired(SourceFields.Type);
            }
            else if (sourceTypeToken is JValue jValue)
            {
                if (jValue.Type == JTokenType.Integer)
                {
                    var candidateSourceType = (SourceType)jValue.ToObject<int>();
                    if (!Enum.IsDefined(typeof(SourceType), candidateSourceType) || candidateSourceType == SourceType.Unknown)
                    {
                        errors.AddOutOfRange(SourceFields.Type);
                    }
                    else
                    {
                        sourceType = candidateSourceType;
                    }
                }
                else
                {
                    errors.AddIntegerExpected(SourceFields.Type);
                }
            }

            return sourceType;
        }
    }
}
