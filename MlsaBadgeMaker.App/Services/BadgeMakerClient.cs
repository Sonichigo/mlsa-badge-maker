using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace MlsaBadgeMaker.App.Services
{
    public class BadgeMakerClient
    {
        private readonly HttpClient _client;
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;

        public BadgeMakerClient(HttpClient client, IAccessTokenProvider tokenProvider, IConfiguration configuration)
        {
            _client = client;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
        }

        public async Task<Stream> CreateBadgeAsync(Stream imageStream)
        {
            var token = await _tokenProvider.RequestAccessToken();
            token.TryGetToken(out var accessToken);
            var jwt = accessToken.Value;

            var response = await _client.PostAsync($"{_configuration["apiEndpoint"]}/api/badge", new MultipartFormDataContent
            {
                { new StringContent(jwt), "token" },
                { new StreamContent(imageStream), "file" }
            });

            response.EnsureSuccessStatusCode();
            var outputStream = await response.Content.ReadAsStreamAsync();
            return outputStream;
        }
    }
}