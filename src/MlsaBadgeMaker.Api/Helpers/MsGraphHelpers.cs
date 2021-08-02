using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MlsaBadgeMaker.Api.Helpers
{
    public static class MsGraphHelpers
    {
        public static GraphServiceClient CreateGraphServiceClient(string accessToken) => 
            new GraphServiceClient(new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return Task.CompletedTask;
            }));
    }
}