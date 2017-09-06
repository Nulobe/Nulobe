using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public interface IFactService
    {
        Task<Fact> GetFactAsync(string id);

        Task<Fact> CreateFactAsync(FactCreate create);

        Task<Fact> UpdateFactAsync(string id, FactCreate create);

        Task DeleteFactAsync(string id);
    }
}
