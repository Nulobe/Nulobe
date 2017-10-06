using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class SourceFieldResolver : ISourceFieldResolver
    {
        private readonly ISourceFieldDictionary _sourceFieldDictionary;
        private readonly IApaSourceFieldDictionary _apaSourceFieldDictionary;

        public SourceFieldResolver(
            ISourceFieldDictionary sourceFieldDictionary,
            IApaSourceFieldDictionary apaSourceFieldDictionary)
        {
            _sourceFieldDictionary = sourceFieldDictionary;
            _apaSourceFieldDictionary = apaSourceFieldDictionary;
        }

        public IEnumerable<string> ResolveFields(SourceType sourceType, ApaSourceType apaSourceType = ApaSourceType.Unknown)
        {
            if (sourceType == SourceType.Apa)
            {
                if (apaSourceType == ApaSourceType.Unknown)
                {
                    throw new ArgumentException("APA source type is required when source type is APA", nameof(apaSourceType));
                }

                return _apaSourceFieldDictionary[apaSourceType];
            }
            else
            {
                return _sourceFieldDictionary[sourceType];
            }
        }
    }
}
