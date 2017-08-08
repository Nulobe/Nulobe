using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class Event
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string FactId { get; set; }

        public EventType Type { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByIp { get; set; }

        public object Data { get; set; }
    }
}
