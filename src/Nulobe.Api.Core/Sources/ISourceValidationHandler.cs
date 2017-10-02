using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public interface ISourceValidationHandler
    {
        SourceType Type { get; }

        IEnumerable<string> Fields { get; }

        Task<SourceValidationResult> IsValidAsync(dynamic source);
    }
}
