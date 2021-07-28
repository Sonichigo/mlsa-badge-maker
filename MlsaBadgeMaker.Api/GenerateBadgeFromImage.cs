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
    public static class GenerateBadgeFromImage
    {
        [FunctionName(nameof(GenerateBadgeFromImage))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "badge")] HttpRequest req,
            ILogger log)
        {
            var imageFormFile = req.Form.Files.GetFile("image");
            var pictureStream = imageFormFile.OpenReadStream();

            IAvatarGenerator generator = new ImageSharpAvatarGenerator();
            var outputStream = await generator.GenerateAsync(pictureStream, MlsaLevel.New);

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}