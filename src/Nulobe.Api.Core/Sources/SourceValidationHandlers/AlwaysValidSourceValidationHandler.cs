using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources.SourceValidationHandlers
{
    public class AlwaysValidSourceValidationHandler : ISourceValidationHandler
    {
        public AlwaysValidSourceValidationHandler(SourceType type, IEnumerable<string> fields)
        {
            Type = type;
            Fields = fields;
        }

        public SourceType Type { get; private set; }

        public IEnumerable<string> Fields { get; private set; }

        public Task<SourceValidationResult> IsValidAsync(dynamic source) => Task.FromResult(SourceValidationResult.Valid());
    }
}
