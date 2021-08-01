using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Repositories;
using MlsaBadgeMaker.Api.Services;
using System.Threading.Tasks;

namespace MlsaBadgeMaker.Api
{

    public class GenerateBadgeFromImage
    {
        private readonly IIntrospectionService _introspectionService;
        private readonly IMembersRepository _membersRepository;
        private readonly IAvatarGenerator _generator;

        public GenerateBadgeFromImage(IIntrospectionService introspectionService,
            IMembersRepository membersRepository,
            IAvatarGenerator generator)
        {
            _introspectionService = introspectionService;
            _membersRepository = membersRepository;
            _generator = generator;
        }

        [FunctionName(nameof(GenerateBadgeFromImage))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "badge")] HttpRequest req,
            ILogger log)
        {
            // Validate token
            if (!req.Form.TryGetValue("token", out var token))
                return new UnauthorizedResult();
            if (!await _introspectionService.IsValidAsync(token))
                return new UnauthorizedResult();

            // Get user
            var username = await _introspectionService.GetPrincipalNameAsync(token);
            var member = await _membersRepository.FindAsync(username);

            // Get image
            var imageFormFile = req.Form.Files.GetFile("image");
            var pictureStream = imageFormFile.OpenReadStream();

            // Generate
            var outputStream = await _generator.GenerateAsync(pictureStream, member.LevelStatus.LevelName);

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}