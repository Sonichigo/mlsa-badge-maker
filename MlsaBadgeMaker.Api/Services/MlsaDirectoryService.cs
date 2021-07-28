using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MlsaBadgeMaker.Api.Data.InfluencerApi;

namespace MlsaBadgeMaker.Api.Services
{
    public class MlsaDirectoryService
    {
        private const string SearchEndpoint = "https://mavenapi-prod.azurewebsites.net/api/UserProfiles/search";

        public async Task<IEnumerable<MlsaMember>> GetAllMembersAsync()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync(SearchEndpoint);

            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<SearchResponse>();

            return response.UserProfiles;
        }
    }
}
