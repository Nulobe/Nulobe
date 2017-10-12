using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class AlwaysValidSourceHandler : ISourceHandler
    {
        public AlwaysValidSourceHandler(SourceType type)
        {
            Type = type;
        }

        public SourceType Type { get; private set; }

        public void PostValidate(JObject source, ModelErrorDictionary errors)
        {
        }

        public virtual Task ProcessAsync(JObject source) => Task.FromResult(0);

        public void PreValidate(JObject source, ModelErrorDictionary errors)
        {
        }
    }
}
