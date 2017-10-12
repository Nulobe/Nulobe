using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class CitationNeededSourceHandler : AlwaysValidSourceHandler
    {
        public CitationNeededSourceHandler() : base(SourceType.CitationNeeded)
        {
        }
    }
}
