using System.Collections.Generic;
using System.Threading.Tasks;
using MlsaBadgeMaker.Api.Data.InfluencerApi;

namespace MlsaBadgeMaker.Api.Repositories
{
    public interface IMembersRepository
    {
        Task<MlsaMember> FindAsync(string emailAddress);
        Task AddOrUpdateAsync(MlsaMember member);
        Task AddOrUpdateRangeAsync(IEnumerable<MlsaMember> members);
    }
}