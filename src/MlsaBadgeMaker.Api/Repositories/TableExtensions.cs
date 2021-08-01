using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table.Protocol;

namespace MlsaBadgeMaker.Api.Repositories
{
    /// <summary>
    /// Taken from https://www.andybutland.dev/2018/11/azure-table-storage-batch-insert-within-operations-limit.html
    /// </summary>
    public static class TableExtensions
    {
        public static async Task<IList<TableResult>> ExecuteBatchAsLimitedBatches(this CloudTable table,
            TableBatchOperation batch)
        {
            if (IsBatchCountUnderSupportedOperationsLimit(batch))
            {
                return await table.ExecuteBatchAsync(batch);
            }

            var result = new List<TableResult>();
            var limitedBatchOperationLists = GetLimitedBatchOperationLists(batch);
            foreach (var limitedBatchOperationList in limitedBatchOperationLists)
            {
                var limitedBatch = CreateLimitedTableBatchOperation(limitedBatchOperationList);
                var limitedBatchResult = await table.ExecuteBatchAsync(limitedBatch);
                result.AddRange(limitedBatchResult);
            }

            return result;
        }


        private static bool IsBatchCountUnderSupportedOperationsLimit(TableBatchOperation batch)
        {
            return batch.Count <= TableConstants.TableServiceBatchMaximumOperations;
        }

        private static IEnumerable<List<TableOperation>> GetLimitedBatchOperationLists(TableBatchOperation batch)
        {
            return batch.ChunkBy(TableConstants.TableServiceBatchMaximumOperations);
        }

        private static TableBatchOperation CreateLimitedTableBatchOperation(IEnumerable<TableOperation> limitedBatchOperationList)
        {
            var limitedBatch = new TableBatchOperation();
            foreach (var limitedBatchOperation in limitedBatchOperationList)
            {
                limitedBatch.Add(limitedBatchOperation);
            }

            return limitedBatch;
        }

        public static List<List<T>> ChunkBy<T>(this IList<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}