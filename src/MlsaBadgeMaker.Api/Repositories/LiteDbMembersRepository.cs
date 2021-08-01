using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MlsaBadgeMaker.Api.Data.InfluencerApi;

namespace MlsaBadgeMaker.Api.Repositories
{
    public class LiteDbMembersRepository : IMembersRepository
    {
        private readonly ILiteDatabase _database;

        public LiteDbMembersRepository(ILiteDatabase database)
        {
            _database = database;
        }

        public Task<MlsaMember> FindAsync(string emailAddress)
        {
            var member = Collection.FindOne(x => x.StudentPartnerEmail == emailAddress);
            return Task.FromResult(member);
        }

        public Task AddOrUpdateAsync(MlsaMember member)
        {
            if (Collection.Exists(x => x.Id == member.Id))
                Collection.Update(member);
            else
                Collection.Insert(member);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task AddOrUpdateRangeAsync(IEnumerable<MlsaMember> members)
        {
            members.ToList().ForEach(x => AddOrUpdateAsync(x));
            return Task.CompletedTask;
        }

        private ILiteCollection<MlsaMember> Collection => _database.GetCollection<MlsaMember>();
    }
}
