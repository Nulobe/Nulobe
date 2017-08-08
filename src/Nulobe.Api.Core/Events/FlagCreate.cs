using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class FlagCreate : IEventCreate
    {
        public string FactId { get; set; }

        public string Description { get; set; }
    }
}
