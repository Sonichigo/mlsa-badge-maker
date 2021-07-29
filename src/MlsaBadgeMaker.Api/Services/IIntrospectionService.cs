using System.Threading.Tasks;

namespace MlsaBadgeMaker.Api.Services
{
    public interface IIntrospectionService
    {
        Task<bool> IsValidAsync(string token);
        Task<string> GetPrincipalNameAsync(string token);
    }
}