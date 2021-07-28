using System.Collections.Generic;

namespace MlsaBadgeMaker.Api.Data.InfluencerApi
{
    public class SearchResponse
    {
        public IEnumerable<MlsaMember> UserProfiles { get; set; }
    }

    public class UserProfileResponse
    {
        public MlsaMember UserProfile { get; set; }
    }
}