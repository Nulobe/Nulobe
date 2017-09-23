using Newtonsoft.Json;
using Nulobe.Api.Core.Sources;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class Fact
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Title { get; set; }

        public string TitleLocalized { get; set; }

        public string Definition { get; set; }

        public string NotesMarkdown { get; set; }

        public string Country { get; set; }

        public IEnumerable<dynamic> Sources { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<string> ReadOnlyTags { get; set; }

        public string Credit { get; set; }

        public string Slug { get; set; }
    }
}
