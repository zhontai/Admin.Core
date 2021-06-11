using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Role.Input;
using AutoMapper;

namespace Admin.Core.Service.Admin.Role
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