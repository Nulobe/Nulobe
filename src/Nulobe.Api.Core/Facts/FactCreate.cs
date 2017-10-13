using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        [StringNotNullOrEmpty]
        public string Title { get; set; }

        [StringNotNullOrEmpty]
        public string DefinitionMarkdown { get; set; }

        public string NotesMarkdown { get; set; }

        public IEnumerable<JObject> Sources { get; set; }

        [EnumerableNotEmpty]
        public IEnumerable<string> Tags { get; set; }

        public string CreditMarkdown { get; set; }

        public string Country { get; set; }
    }
}
