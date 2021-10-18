using AutoMapper;
using ZhonTai.Plate.Admin.Domain.Role;
using ZhonTai.Plate.Admin.Service.Role.Input;

namespace ZhonTai.Plate.Admin.Service.Role
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<RoleAddInput, RoleEntity>();
            CreateMap<RoleUpdateInput, RoleEntity>();
        }
    }
}