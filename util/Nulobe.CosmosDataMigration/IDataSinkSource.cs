using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.CosmosDataMigration
{
    public interface IDataSinkSource : IDataSink
    {
        DataSinkArgumentCollection GetSourceSinkArguments(string databaseName, string collectionName);
    }
}
