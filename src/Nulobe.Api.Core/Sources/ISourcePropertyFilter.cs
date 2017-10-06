using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Core.Sources
{
    public interface ISourcePropertyFilter
    {
        JObject GetFilteredSource(JObject source);
    }
}
