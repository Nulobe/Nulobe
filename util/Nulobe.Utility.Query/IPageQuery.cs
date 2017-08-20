using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Query
{
    public interface IPageQuery
    {
        string PageNumber { get; set; }

        string PageSize { get; set; }
    }
}
