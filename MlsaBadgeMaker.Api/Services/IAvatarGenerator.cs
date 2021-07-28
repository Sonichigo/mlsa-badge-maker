using System.IO;
using System.Threading.Tasks;

namespace MlsaBadgeMaker.Api.Services
{
    public interface IAvatarGenerator
    {
        Task<Stream> GenerateAsync(Stream imageStream, string milestoneName);
    }
}