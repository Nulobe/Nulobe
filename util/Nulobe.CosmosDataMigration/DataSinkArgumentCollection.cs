using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.CosmosDataMigration
{
    public class DataSinkArgumentCollection
    {
        private readonly Dictionary<string, string> _args;

        public DataSinkArgumentCollection(Dictionary<string, string> args)
        {
            _args = args;
        }

        public string ToString(string prefix) => string.Join(" ", _args.Select(kvp =>
            ArgumentEncoder.Encode($"{prefix}.{kvp.Key}", kvp.Value))); 
    }
}
