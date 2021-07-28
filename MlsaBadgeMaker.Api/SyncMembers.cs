using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
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
        public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo myTimer,
            ILogger log)
        {
            var members = await _directoryService.GetAllMembersAsync();

            await _membersRepository.AddOrUpdateRangeAsync(members);
        }
    }
}
