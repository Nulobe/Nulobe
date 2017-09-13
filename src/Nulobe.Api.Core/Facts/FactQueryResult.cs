using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class FactQueryResult
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<Fact> Facts { get; set; }
    }
}
