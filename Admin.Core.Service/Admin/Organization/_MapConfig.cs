using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Organization.Input;
using AutoMapper;

namespace Admin.Core.Service.Admin.Organization
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<OrganizationAddInput, OrganizationEntity>();
            CreateMap<OrganizationUpdateInput, OrganizationEntity>();
        }
    }
}