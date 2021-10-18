using ZhonTai.Plate.Personnel.Domain;
using ZhonTai.Plate.Personnel.Service.Organization.Input;
using AutoMapper;

namespace ZhonTai.Plate.Personnel.Service.Organization
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