using System.Collections.Generic;

namespace MlsaBadgeMaker.Api.Data.InfluencerApi
{
    public class SearchResponse
    {
        public IEnumerable<MlsaMember> UserProfiles { get; set; }
    }
}