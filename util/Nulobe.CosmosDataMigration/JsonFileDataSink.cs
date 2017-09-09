using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.CosmosDataMigration
{
    public class JsonFileDataSink : IDataSinkSource, IDataSinkTarget
    {
        private readonly JsonFileDataSinkOptions _options;

        public JsonFileDataSink(JsonFileDataSinkOptions options)
        {
            _options = options;
        }

        public string SinkName => "JsonFile";

        public DataSinkArgumentCollection GetSourceSinkArguments(string databaseName, string collectionName) =>
            GetSinkArguments("Files", databaseName, collectionName);

        public DataSinkArgumentCollection GetTargetSinkArguments(string databaseName, string collectionName) =>
            GetSinkArguments("File", databaseName, collectionName);

        private DataSinkArgumentCollection GetSinkArguments(string fileArgumentName, string databaseName, string collectionName) =>
            new DataSinkArgumentCollection(new Dictionary<string, string>()
            {
                { fileArgumentName, Path.Combine(_options.Directory, $"{databaseName}.{collectionName}.json") }
            });
    }
}
