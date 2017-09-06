using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Facts
{
    public class FactAudit : IAuditableAction
    {
        public FactData CurrentValue { get; set; }

        public FactData PreviousValue { get; set; }

        public string ActionName { get; set; }

        public DateTime Actioned { get; set; }

        public string ActionedById { get; set; }

        public string ActionedByRemoteIp { get; set; }
    }
}
