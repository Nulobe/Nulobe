using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Tags
{
    public interface ITagMemoryRepository
    {
        Task<IEnumerable<Tag>> GetTagsAsync();
    }
}
