using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class FactCreate
    {
        [StringNotNullOrEmpty]
        public string Title { get; set; }

        [StringNotNullOrEmpty]
        public string Definition { get; set; }

        public string Notes { get; set; }

        public IEnumerable<Source> Sources { get; set; } = Enumerable.Empty<Source>();

        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        public string Credit { get; set; }
    }
}
