using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class Event : TableEntity
    {
        public string FactId { get; set; }

        public string EventType { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByIp { get; set; }

        public string DataJson { get; set; }
    }
}
