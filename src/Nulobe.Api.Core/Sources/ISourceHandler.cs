using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public interface ISourceHandler
    {
        SourceType Type { get; }

        void PreValidate(JObject source, ModelErrorDictionary errors);

        Task ProcessAsync(JObject source);

        void PostValidate(JObject source, ModelErrorDictionary errors);
    }
}
