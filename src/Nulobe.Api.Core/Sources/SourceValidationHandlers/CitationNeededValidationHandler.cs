using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.SourceValidationHandlers
{
    public class CitationNeededValidationHandler : AlwaysValidSourceValidationHandler
    {
        public CitationNeededValidationHandler() : base(SourceType.CitationNeeded, Enumerable.Empty<string>())
        {
        }
    }
}
