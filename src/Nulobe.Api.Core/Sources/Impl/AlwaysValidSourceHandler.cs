using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class AlwaysValidSourceHandler : BaseSourceHandler
    {
        public AlwaysValidSourceHandler(SourceType type) : base(type)
        {
        }
    }
}
