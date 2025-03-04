using Mapster;
using ZhonTai.Admin.Services.Tenant.Dto;

namespace ZhonTai.Admin.Services.Tenant;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<TenantGetPageOutput, TenantGetPageOutput>()
        .Map(dest => dest.PkgNames, src => src.Pkgs.Select(a => a.Name));
    }
}