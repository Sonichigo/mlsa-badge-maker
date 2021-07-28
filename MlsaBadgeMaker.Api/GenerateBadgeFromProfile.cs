using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Properties;
using MlsaBadgeMaker.Api.Services;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MlsaBadgeMaker.Api
{
    public static class GenerateBadgeFromProfile
    {
        [FunctionName(nameof(GenerateBadgeFromProfile))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "badge/{emailAddress}")] HttpRequest req,
            [FromRoute] string emailAddress,
            ILogger log)
        {
            using var client = new HttpClient();

            var service = new MlsaDirectoryService(client);
            var members = await service.GetAllMembersAsync();
            var member = members.Single(x => x.StudentPartnerEmail == emailAddress);
            var pictureStream = await client.GetStreamAsync(member.ProfilePictureUrl);

            IAvatarGenerator generator = new ImageSharpAvatarGenerator();
            var outputStream = await generator.GenerateAsync(pictureStream, member.LevelStatus.LevelName);

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}
