using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.SlugBuilder
{
    public interface ISlugBuilder
    {
        ISlugBuilder Add(Guid input);

        ISlugBuilder Add(long input);

        ISlugBuilder Add(int input);

        ISlugBuilder Add(string input);

        ISlugBuilder Add(string format, params object[] args);
    }
}
