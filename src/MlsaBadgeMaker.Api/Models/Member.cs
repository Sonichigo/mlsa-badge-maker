using Microsoft.Azure.Cosmos.Table;
using MlsaBadgeMaker.Api.Data.InfluencerApi;

namespace MlsaBadgeMaker.Api.Models
{
    public class Member : TableEntityAdapter<MlsaMember>
    {
        public Member()
        {
        }

        public Member(MlsaMember member)
        {
            OriginalEntity = member;
            PartitionKey = "mlsa";
            RowKey = member.StudentPartnerEmail;
        }
    }
}
