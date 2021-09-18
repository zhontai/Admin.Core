using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Tenant.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.Tenant
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