using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Role.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.Role
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