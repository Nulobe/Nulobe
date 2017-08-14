using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class Source
    {
        [StringNotNullOrEmpty]
        public string Url { get; set; }

        public string Description { get; set; }
    }
}
