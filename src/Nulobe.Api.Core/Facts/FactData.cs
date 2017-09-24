using Newtonsoft.Json;
using Nulobe.Api.Core.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class FactData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Definition { get; set; }

        public string NotesMarkdown { get; set; }

        public string Country { get; set; }

        public IEnumerable<IDictionary<string, object>> Sources { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Credit { get; set; }

        public string Slug { get; set; }

        public IEnumerable<FactDataSlugAudit> SlugHistory { get; set; }
    }
}
