using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MlsaBadgeMaker.App.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace MlsaBadgeMaker.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<BadgeMakerClient>();

            builder.Services.AddSingleton<HttpClient>();

            builder.Services.AddMsalAuthentication(options =>
            {
                options.ProviderOptions.DefaultAccessTokenScopes.Add("User.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("User.ReadWrite");
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            await builder.Build().RunAsync();
        }
    }
}
