using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Tenant.Input;

namespace Admin.Core.Service.Admin.Tenant
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
