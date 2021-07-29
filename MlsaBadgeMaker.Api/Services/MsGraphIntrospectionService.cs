using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MlsaBadgeMaker.Api.Services
{
    public class MsGraphIntrospectionService : IIntrospectionService
    {
        /// <inheritdoc />
        public async Task<bool> IsValidAsync(string token)
        {
            var graphService = CreateGraphServiceClient(token);

            try
            {
                var name = await GetPrincipalNameAsync(token);
                return !string.IsNullOrEmpty(name) && name.EndsWith("studentambassadors.com");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetPrincipalNameAsync(string token)
        {
            var graphService = CreateGraphServiceClient(token);

            try
            {
                var response = await graphService.Me.Request().GetResponseAsync();
                response.ToHttpResponseMessage().EnsureSuccessStatusCode();

                var user = await response.Content.ReadAsAsync<User>();
                return user.UserPrincipalName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private GraphServiceClient CreateGraphServiceClient(string accessToken) => 
            new GraphServiceClient(new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return Task.CompletedTask;
            }));
    }
}