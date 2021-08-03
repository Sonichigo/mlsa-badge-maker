using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Repositories;
using MlsaBadgeMaker.Api.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MlsaBadgeMaker.Api
{
    public class GenerateBadgeFromProfile
    {
        private readonly IMembersRepository _membersRepository;
        private readonly IIntrospectionService _introspectionService;
        private readonly HttpClient _client;

        public GenerateBadgeFromProfile(IMembersRepository membersRepository,
            IIntrospectionService introspectionService,
            IHttpClientFactory httpClientFactory)
        {
            _membersRepository = membersRepository;
            _introspectionService = introspectionService;
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(GenerateBadgeFromProfile))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "badge")] HttpRequest req,
            ILogger log)
        {
            // Validate token
            if (!req.Headers.TryGetValue("token", out var token))
                return new UnauthorizedResult();
            if (!await _introspectionService.IsValidAsync(token))
                return new UnauthorizedResult();

            // Get user 
            var name = await _introspectionService.GetPrincipalNameAsync(token);
            var member = await _membersRepository.FindAsync(name);
            if (member is null)
                return new BadRequestObjectResult("Your Student Ambassador profile could not be found to determine your milestone.");

            if (string.IsNullOrEmpty(member.ProfilePictureUrl))
                return new BadRequestObjectResult("Default profile image is not set in your Student Ambassador profile. Try to upload a custom image instead.");

            // Generate
            var pictureStream = await _client.GetStreamAsync(member.ProfilePictureUrl);

            IAvatarGenerator generator = new ImageSharpAvatarGenerator();
            var outputStream = await generator.GenerateAsync(pictureStream, member.LevelStatus.LevelName);

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}
