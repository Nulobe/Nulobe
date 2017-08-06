using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public interface IFactQueryService
    {
        Task<IEnumerable<Fact>> QueryFactsAsync(FactQuery query);
    }
}
