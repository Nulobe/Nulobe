using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.CosmosDataMigration
{
    public class CosmosDataMigrationToolClient
    {
        public void Transfer(IDataSinkSource source, IDataSinkTarget target, string databaseName, string collectionName)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(Directory.GetCurrentDirectory(), "dt-1.7", "dt.exe"),
                Arguments = string.Join(" ", new string[]
                {
                    ArgumentEncoder.Encode("s", source.SinkName),
                    source.GetSourceSinkArguments(databaseName, collectionName).ToString("s"),
                    ArgumentEncoder.Encode("t", target.SinkName),
                    target.GetTargetSinkArguments(databaseName, collectionName).ToString("t"),
                }),
                UseShellExecute = false
            };

            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }
        }
    }
}
