using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public class CloudBlob
    {
        public string Path { get; set; }

        public string ContentType { get; set; }

        public Stream Stream { get; set; }
    }
}
