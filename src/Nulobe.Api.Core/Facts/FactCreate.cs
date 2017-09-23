using Newtonsoft.Json;
using Nulobe.Api.Core.Sources;
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
        public string Title { get; set; }

        public string Definition { get; set; }

        public string NotesMarkdown { get; set; }

        public IEnumerable<dynamic> Sources { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Credit { get; set; }

        public string Country { get; set; }
    }
}
