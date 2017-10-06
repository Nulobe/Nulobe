using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public interface ISourceFieldResolver
    {
        IEnumerable<string> ResolveFields(SourceType sourceType, ApaSourceType apaSourceType = ApaSourceType.Unknown);
    }
}
