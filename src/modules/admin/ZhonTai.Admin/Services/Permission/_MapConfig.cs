using Mapster;
using System.Linq;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Services.Permission.Dto;

namespace ZhonTai.Admin.Services.Admin.Permission;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<PermissionEntity, PermissionGetDotOutput>()
        .Map(dest => dest.ApiIds, src => src.Apis.Select(a => a.Id));
    }
}