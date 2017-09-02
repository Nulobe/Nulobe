﻿using Newtonsoft.Json;
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

        public string Definition { get; set; }

        public string NotesMarkdown { get; set; }

        public IEnumerable<Source> Sources { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Credit { get; set; }

        public string Slug { get; set; }

        public IEnumerable<SlugAudit> SlugHistory { get; set; }
    }

    public class SlugAudit
    {
        public string Slug { get; set; }

        public DateTime Created { get; set; }
    }

}
