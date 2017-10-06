using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public interface ISourceFieldDictionary
    {
        IEnumerable<string> this[SourceType key] { get; }

        IDictionary<SourceType, IEnumerable<string>> GetDictionary();
    }
}
