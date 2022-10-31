using Mapster;
using System.Linq;
using ZhonTai.Admin.Services.User.Dto;

namespace ZhonTai.Admin.Services.User;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<UserGetPageOutput, UserGetPageOutput>()
        .Map(dest => dest.RoleNames, src => src.Roles.Select(a => a.Name));
    }
}