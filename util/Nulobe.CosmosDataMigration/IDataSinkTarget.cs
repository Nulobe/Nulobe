using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.CosmosDataMigration
{
    public interface IDataSinkTarget : IDataSink
    {
        DataSinkArgumentCollection GetTargetSinkArguments(string databaseName, string collectionName);
    }
}
