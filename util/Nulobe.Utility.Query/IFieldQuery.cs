using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Query
{
    public interface IFieldQuery<T>
    {
        string Fields { get; set; }
    }
}
