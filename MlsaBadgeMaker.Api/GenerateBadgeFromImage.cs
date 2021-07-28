using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Data.InfluencerApi;
using MlsaBadgeMaker.Api.Services;

namespace MlsaBadgeMaker.Api
{

    public class GenerateBadgeFromImage
    {
        private readonly IIntrospectionService _introspectionService;
        private readonly IAvatarGenerator _generator;

        public GenerateBadgeFromImage(IIntrospectionService introspectionService, IAvatarGenerator generator)
        {
            _introspectionService = introspectionService;
            _generator = generator;
        }

        [FunctionName(nameof(GenerateBadgeFromImage))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "badge")] HttpRequest req,
            ILogger log)
        {
            // Validate token
            var token = req.Form["token"];
            if (!await _introspectionService.IsValidAsync(token))
                return new UnauthorizedResult();

            // Get image
            var imageFormFile = req.Form.Files.GetFile("image");
            var pictureStream = imageFormFile.OpenReadStream();

            // Generate
            var outputStream = await _generator.GenerateAsync(pictureStream, MlsaLevel.New);

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}