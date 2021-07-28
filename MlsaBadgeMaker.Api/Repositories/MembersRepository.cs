using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MlsaBadgeMaker.Api.Data.InfluencerApi;

namespace MlsaBadgeMaker.Api.Repositories
{
    public class MembersRepository
    {
        private readonly ILiteDatabase _database;

        public MembersRepository(ILiteDatabase database)
        {
            _database = database;
        }

        public Task<MlsaMember> FindAsync(string emailAddress)
        {
            var member = Collection.FindOne(x => x.StudentPartnerEmail == emailAddress);
            return Task.FromResult(member);
        }

        public Task AddAsync(MlsaMember member)
        {
            Collection.Insert(member);
            return Task.CompletedTask;
        }

        private ILiteCollection<MlsaMember> Collection => _database.GetCollection<MlsaMember>();
    }
}
