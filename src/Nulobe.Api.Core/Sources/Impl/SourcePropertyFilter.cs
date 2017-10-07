using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class SourcePropertyFilter : ISourcePropertyFilter
    {
        private readonly ISourceFieldResolver _sourceFieldResolver;

        public SourcePropertyFilter(
            ISourceFieldResolver sourceFieldResolver)
        {
            _sourceFieldResolver = sourceFieldResolver;
        }

        public JObject GetFilteredSource(JObject source)
        {
            var sourceType = GetSourceType(source);
            IEnumerable<string> fields = null;

            if (sourceType == SourceType.Apa)
            {
                fields = _sourceFieldResolver.ResolveFields(sourceType, GetApaSourceType(source));
            }
            else
            {
                fields = _sourceFieldResolver.ResolveFields(sourceType);
            }

            var result = new JObject();
            foreach (var kvp in source)
            {
                if (fields.Contains(kvp.Key))
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }

            return result;
        }


        #region Helpers

        private SourceType GetSourceType(JObject source)
            => (SourceType)source.SelectToken(SourceFields.Type).ToObject<int>();

        private ApaSourceType GetApaSourceType(JObject source)
            => (ApaSourceType)source.SelectToken(SourceFields.ApaType).ToObject<int>();

        #endregion
    }
}
