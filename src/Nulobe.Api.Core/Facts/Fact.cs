﻿using Newtonsoft.Json;
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

        public IEnumerable<Source> Sources { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Credit { get; set; }
    }

    public class Source
    {
        public string Description { get; set; }

        public string Url { get; set; }
    }
}
