using Microsoft.Azure.Cosmos.Table;
using MlsaBadgeMaker.Api.Data.InfluencerApi;
using MlsaBadgeMaker.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MlsaBadgeMaker.Api.Repositories
{
    public class TableMembersRepository : IMembersRepository
    {
        private readonly CloudTableClient _client;

        public TableMembersRepository(CloudTableClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<MlsaMember> FindAsync(string emailAddress)
        {
            var operation = TableOperation.Retrieve<Member>("mlsa", emailAddress);
            var member = await Table.ExecuteAsync(operation);
            
            var entity = member.Result as Member;
            return entity?.OriginalEntity;
        }

        /// <inheritdoc />
        public Task AddOrUpdateAsync(MlsaMember member) => Table.ExecuteAsync(TableOperation.InsertOrReplace(new Member(member)));

        /// <inheritdoc />
        public async Task AddOrUpdateRangeAsync(IEnumerable<MlsaMember> members)
        {
            var operations = members.Select(x => TableOperation.InsertOrReplace(new Member(x))).ToList();

            var batch = new TableBatchOperation();
            operations.ForEach(x => batch.Add(x));

            await Table.ExecuteBatchAsLimitedBatches(batch);
        }

        private CloudTable Table
        {
            get
            {
                var table = _client.GetTableReference("members");
                table.CreateIfNotExists();

                return table;
            }
        }
    }
}