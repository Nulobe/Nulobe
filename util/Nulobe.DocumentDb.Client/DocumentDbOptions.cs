using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.DocumentDb.Client
{
    public class DocumentDbOptions
    {
        public string ConnectionString { get; set; }

        public string ReadOnlyConnectionString { get; set; }
    }
}
