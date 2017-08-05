using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Core
{
    public class TagQuery
    {
        public string SearchPattern { get; set; } = string.Empty;

        public string Fields { get; set; } = $"{nameof(Tag.Text)}";

        public string OrderBy { get; set; } = $"{nameof(Tag.Text)}";
    }
}
