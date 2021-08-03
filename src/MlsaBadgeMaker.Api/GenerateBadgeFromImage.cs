using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Repositories;
using MlsaBadgeMaker.Api.Services;
using System.Threading.Tasks;
using MlsaBadgeMaker.Api.Exceptions;

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
            if (member is null)
                return new BadRequestObjectResult("Your Student Ambassador profile could not be found to determine your milestone.");

            // Get image
            var imageFormFile = req.Form.Files.GetFile("image");
            if (imageFormFile.Length > 5000000)
                return new BadRequestObjectResult("The image file size must be less than 5 MB.");

            var pictureStream = imageFormFile.OpenReadStream();

            // Generate
            try
            {
                var outputStream = await _generator.GenerateAsync(pictureStream, member.LevelStatus.LevelName);

                log.LogInformation("Generated {milestone} badge for user {username}",
                    member.LevelStatus.LevelName, username);
                return new FileStreamResult(outputStream, "image/png");
            }
            catch (ImageManipulationException ex)
            {
                log.LogWarning(ex, "Manipulation exception occurred for {username}", username);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}