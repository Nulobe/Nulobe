using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Events
{
    public class VoteCreate : IEventCreate
    {
        public string FactId { get; set; }
    }
}
