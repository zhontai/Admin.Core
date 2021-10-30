using ZhonTai.Common.Domain.Dto;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.Service.Auth.Input;

namespace ZhonTai.Plate.Admin.Service.Auth
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public interface IAuthService
    {
        Task<IResultOutput> LoginAsync(AuthLoginInput input);

        Task<IResultOutput> GetUserInfoAsync();

        Task<IResultOutput> GetPassWordEncryptKeyAsync();
    }
}