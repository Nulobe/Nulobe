using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Tags
{
    public interface ITagQueryService
    {
        Task<IEnumerable<Tag>> QueryTagsAsync(TagQuery query);
    }
}
