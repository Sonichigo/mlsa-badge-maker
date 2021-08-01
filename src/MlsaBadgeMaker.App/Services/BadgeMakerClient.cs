using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<Stream> CreateBadgeAsync()
        {
            var token = await GetTokenAsync();
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, 
                $"{_configuration["ApiEndpoint"]}/api/badge")
                {
                    Headers = {{"token", token}}
                });

            response.EnsureSuccessStatusCode();
            var outputStream = await response.Content.ReadAsStreamAsync();
            return outputStream;
        }

        public async Task<Stream> CreateBadgeAsync(IBrowserFile file)
        {
            var imageStream = file.OpenReadStream();

            var jwt = await GetTokenAsync();

            var response = await _client.PostAsync($"{_configuration["ApiEndpoint"]}/api/badge", new MultipartFormDataContent
            {
                { new StringContent(jwt), "token" },
                { new StreamContent(imageStream), "image", file.Name }
            });

            response.EnsureSuccessStatusCode();
            var outputStream = await response.Content.ReadAsStreamAsync();
            return outputStream;
        }

        private async Task<string> GetTokenAsync()
        {
            var token = await _tokenProvider.RequestAccessToken();
            token.TryGetToken(out var accessToken);
            var jwt = accessToken.Value;
            return jwt;
        }
    }
}