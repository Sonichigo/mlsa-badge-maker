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
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var graphService = new GraphServiceClient(new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Task.CompletedTask;
            }));

            try
            {
                var response = await graphService.Me.Request().GetResponseAsync();
                response.ToHttpResponseMessage().EnsureSuccessStatusCode();

                var user = await response.Content.ReadAsAsync<User>();
                return user != null && user.UserPrincipalName.EndsWith("studentambassadors.com");
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}