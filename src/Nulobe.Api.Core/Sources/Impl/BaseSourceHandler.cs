using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;

namespace Nulobe.Api.Core.Sources.Impl
{
    public abstract class BaseSourceHandler : ISourceHandler
    {
        public SourceType Type { get; private set; }

        public BaseSourceHandler(SourceType type)
        {
            Type = type;
        }

        public virtual void PreValidate(JObject source, ModelErrorDictionary errors)
        {
        }

        public virtual Task ProcessAsync(JObject source)
            => Task.FromResult(0);

        public virtual void PostValidate(JObject source, ModelErrorDictionary errors)
        {
        }
    }
}
