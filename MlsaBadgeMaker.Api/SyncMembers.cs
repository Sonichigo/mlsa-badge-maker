using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Data.InfluencerApi;
using MlsaBadgeMaker.Api.Services;

namespace MlsaBadgeMaker.Api
{
    public static class SyncMembers
    {
        [FunctionName("SyncMembers")]
        public static async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo myTimer,
            ILogger log)
        {
            var directoryService = new MlsaDirectoryService(new HttpClient());
            var members = await directoryService.GetAllMembersAsync();
        }
    }
}
