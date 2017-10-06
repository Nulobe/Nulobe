using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public interface IApaSourceFieldDictionary
    {
        IEnumerable<string> this[ApaSourceType key] { get; }
    }
}
