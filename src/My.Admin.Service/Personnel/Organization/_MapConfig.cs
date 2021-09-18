using My.Admin.Domain.Personnel;
using My.Admin.Service.Personnel.Organization.Input;
using AutoMapper;

namespace My.Admin.Service.Personnel.Organization
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<OrganizationAddInput, OrganizationEntity>();
            //修改
            CreateMap<OrganizationUpdateInput, OrganizationEntity>();
        }
    }
}