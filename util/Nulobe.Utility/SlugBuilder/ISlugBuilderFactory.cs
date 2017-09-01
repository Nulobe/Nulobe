using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.SlugBuilder
{
    public interface ISlugBuilderFactory
    {
        ISlugBuilder Create();
    }
}
