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
        private readonly HttpClient _client;

        public const string UserProfilesEndpoint = "https://mavenapi-prod.azurewebsites.net/api/UserProfiles";
        public const string SearchEndpoint = UserProfilesEndpoint + "/search";

        public MlsaDirectoryService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<IEnumerable<MlsaMember>> GetAllMembersAsync()
        {
            var result = await _client.GetAsync(SearchEndpoint);

            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<SearchResponse>();

            return response.UserProfiles;
        }

        public async Task<MlsaMember> GetMemberAsync(string emailAddress)
        {
            var result = await _client.GetAsync($"{UserProfilesEndpoint}/{emailAddress}");

            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<UserProfileResponse>();

            return response.UserProfile;
        }
    }
}
