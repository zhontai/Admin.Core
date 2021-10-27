using Mapster;
using System.Linq;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Service.Permission.Output;

namespace ZhonTai.Plate.Admin.Service.Admin.Permission
{
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
}