using Microsoft.Azure.Cosmos.Table;
using MlsaBadgeMaker.Api.Data.InfluencerApi;

namespace MlsaBadgeMaker.Api
{
    public class MemberEntity : TableEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StudentPartnerEmail { get; set; }

        public MlsaLevel LevelStatus { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}