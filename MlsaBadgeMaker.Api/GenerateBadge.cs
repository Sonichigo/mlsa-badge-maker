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
    public static class GenerateBadge
    {
        [FunctionName("badge")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string email = req.Query["email"];

            using var client = new HttpClient();

            var service = new MlsaDirectoryService(client);
            var members = await service.GetAllMembersAsync();
            
            var member = members.Single(x => x.StudentPartnerEmail == email);


            var picture = await client.GetStreamAsync(member.ProfilePictureUrl);

            using var image = await Image.LoadAsync(picture);

            var badgeBuffer = member.LevelStatus.LevelName switch
            {
                "New" => Resources.MSLearn_SA_Profile_Badge_Overlay_GENERIC,
                "Alpha" => Resources.MSLearn_SA_Profile_Badge_Overlay_ALPHA,
                "Beta" => Resources.MSLearn_SA_Profile_Badge_Overlay_BETA,
                "Gold" => Resources.MSLearn_SA_Profile_Badge_Overlay_GOLD,
                _ => throw new ArgumentOutOfRangeException()
            };

            using var badgeImage = await Image.LoadAsync(new MemoryStream(badgeBuffer));
            badgeImage.Mutate(x => x.Resize(image.Size()));

            image.Mutate(x => x.DrawImage(badgeImage, new Point(0), 1));

            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, PngFormat.Instance);

            outputStream.Position = 0;

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}
