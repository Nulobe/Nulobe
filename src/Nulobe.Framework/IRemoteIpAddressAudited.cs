using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public class IRemoteIpAddressAudited
    {
        public DateTime EventTime { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}
