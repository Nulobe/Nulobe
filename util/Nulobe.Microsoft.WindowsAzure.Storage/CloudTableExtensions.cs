using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.Storage.Table
{
    public static class CloudTableExtensions
    {
        public static Task InsertOrReplaceBatchAsync<T>(
            this CloudTable cloudTable,
            IEnumerable<T> entities,
            int batchSize = 100)
            where T : ITableEntity => cloudTable.ExecuteBatchAsync(entities, (o, e) => o.InsertOrReplace(e), batchSize);

        public static Task DeleteBatchAsync<T>(
            this CloudTable cloudTable,
            IEnumerable<T> entities,
            int batchSize = 100)
            where T : ITableEntity => cloudTable.ExecuteBatchAsync(entities, (o, e) => o.Delete(e), batchSize);

        public static async Task<IEnumerable<T>> ListAsync<T>(
            this CloudTable cloudTable)
            where T : ITableEntity, new()
        {
            var results = Enumerable.Empty<T>();
            var continuationToken = new TableContinuationToken();
            while (continuationToken != null)
            {
                var segment = await cloudTable.CreateQuery<T>().ExecuteSegmentedAsync(continuationToken);
                results = segment.Results.Concat(results);
                continuationToken = segment.ContinuationToken;
            }
            return results;
        }


        #region

        private static async Task ExecuteBatchAsync<T>(
            this CloudTable cloudTable,
            IEnumerable<T> entities,
            Action<TableBatchOperation, ITableEntity> batchAction,
            int batchSize = 100)
            where T : ITableEntity
        {
            var batchedEntities = entities
                .Select((entity, index) => new
                {
                    Entity = entity,
                    Index = index
                })
                .ToLookup(
                    x => (int)Math.Floor((double)x.Index / batchSize),
                    x => x.Entity);

            foreach (var batch in batchedEntities)
            {
                var batchOperation = new TableBatchOperation();
                foreach (var e in batch)
                {
                    batchAction(batchOperation, e);
                }
                await cloudTable.ExecuteBatchAsync(batchOperation);
            }
        }

        #endregion
    }
}
