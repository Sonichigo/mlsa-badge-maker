using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MlsaBadgeMaker.Api.Data.InfluencerApi;
using MlsaBadgeMaker.Api.Repositories;
using MlsaBadgeMaker.Api.Services;

namespace MlsaBadgeMaker.Api
{
    public class SyncMembers
    {
        private readonly IMembersRepository _membersRepository;
        private readonly MlsaDirectoryService _directoryService;

        public SyncMembers(IMembersRepository membersRepository, MlsaDirectoryService directoryService)
        {
            _membersRepository = membersRepository;
            _directoryService = directoryService;
        }

        [FunctionName("SyncMembers")]
#if RELEASE
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sync/members")] HttpRequest req,
#else
        public async Task Run(
            [TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo myTimer,
#endif
            ILogger log)
        {
            log.LogInformation("Fetching members...");

            var members = await _directoryService.GetAllMembersAsync();
            var mlsaMembers = members.ToList();
            log.LogInformation("Fetched {0} members from API", mlsaMembers.Count);

            await _membersRepository.AddOrUpdateRangeAsync(mlsaMembers);
            log.LogInformation("Synced all {0} members", mlsaMembers.Count);

#if RELEASE
            return new OkResult();
#endif
        }
    }
}
