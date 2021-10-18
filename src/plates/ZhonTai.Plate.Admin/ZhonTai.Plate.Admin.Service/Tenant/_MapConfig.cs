using AutoMapper;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Service.Tenant.Input;

namespace ZhonTai.Plate.Admin.Service.Admin.Tenant
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<TenantAddInput, TenantEntity>();
            CreateMap<TenantUpdateInput, TenantEntity>();
        }
    }
}