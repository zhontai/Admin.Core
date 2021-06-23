using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Organization.Input;
using AutoMapper;

namespace Admin.Core.Service.Personnel.Organization
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