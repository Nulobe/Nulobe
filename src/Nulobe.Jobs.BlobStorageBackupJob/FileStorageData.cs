using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Jobs.BlobStorageBackupJob
{
    public class FileStorageData
    {
        public string Name { get; set; }

        public IEnumerable<string> KeyParts { get; set; }

        public string ContentType { get; set; }

        public long Length { get; set; }

        public Stream Stream { get; set; }
    }
}
