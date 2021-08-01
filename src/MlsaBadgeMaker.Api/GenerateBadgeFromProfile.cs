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
using MlsaBadgeMaker.Api.Repositories;
using MlsaBadgeMaker.Api.Services;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MlsaBadgeMaker.Api
{
    public class GenerateBadgeFromProfile
    {
        private readonly IMembersRepository _membersRepository;
        private readonly HttpClient _client;

        public GenerateBadgeFromProfile(IMembersRepository membersRepository, IHttpClientFactory httpClientFactory)
        {
            _membersRepository = membersRepository;
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(GenerateBadgeFromProfile))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "badge/{emailAddress}")] HttpRequest req,
            [FromRoute] string emailAddress,
            ILogger log)
        {
            var member = await _membersRepository.FindAsync(emailAddress);
            if (member is null)
                return new NotFoundResult();

            var pictureStream = await _client.GetStreamAsync(member.ProfilePictureUrl);

            IAvatarGenerator generator = new ImageSharpAvatarGenerator();
            var outputStream = await generator.GenerateAsync(pictureStream, member.LevelStatus.LevelName);

            return new FileStreamResult(outputStream, "image/png");
        }
    }
}
