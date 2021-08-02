using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Helpers;
using MlsaBadgeMaker.Api.Services;

namespace MlsaBadgeMaker.Api
{
    public class ApplyBadgeToTeams
    {
        private readonly IIntrospectionService _introspectionService;

        public ApplyBadgeToTeams(IIntrospectionService introspectionService)
        {
            _introspectionService = introspectionService;
        }

        [FunctionName(nameof(ApplyBadgeToTeams))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "apply/teams")] HttpRequest req,
            ILogger log)
        {
            // Validate token
            if (!req.Headers.TryGetValue("token", out var token))
                return new UnauthorizedResult();
            if (!await _introspectionService.IsValidAsync(token))
                return new UnauthorizedResult();

            // Get image
            var image = req.Form.Files.GetFile("image");

            try
            {
                // Create service client
                var client = MsGraphHelpers.CreateGraphServiceClient(token);
                await client.Me.Photo.Content.Request().PutAsync(image.OpenReadStream());
            }
            catch (Exception e)
            {
                return new BadRequestErrorMessageResult(e.Message);
            }

            return new AcceptedResult();
        }
    }
}