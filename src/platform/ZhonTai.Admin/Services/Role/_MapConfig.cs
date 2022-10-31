using Mapster;
using System.Linq;
using ZhonTai.Admin.Services.Role.Dto;

namespace ZhonTai.Admin.Services.Role;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<RoleGetOutput, RoleGetOutput>()
        .Map(dest => dest.OrgIds, src => src.Orgs.Select(a => a.Id));
    }
}