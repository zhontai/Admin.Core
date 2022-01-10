using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service.Auth.Dto;

namespace ZhonTai.Plate.Admin.Service.Auth
{
    public interface IAuthService
    {
        Task<IResultOutput> LoginAsync(AuthLoginInput input);

        Task<IResultOutput> GetUserInfoAsync();

        Task<IResultOutput> GetPasswordEncryptKeyAsync();
    }
}