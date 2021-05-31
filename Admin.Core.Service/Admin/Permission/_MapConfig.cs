using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Permission.Input;

namespace Admin.Core.Service.Admin.Permission
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<PermissionAddGroupInput, PermissionEntity>();
            CreateMap<PermissionAddMenuInput, PermissionEntity>();
            CreateMap<PermissionAddApiInput, PermissionEntity>();
            CreateMap<PermissionAddDotInput, PermissionEntity>();

            CreateMap<PermissionUpdateGroupInput, PermissionEntity>();
            CreateMap<PermissionUpdateMenuInput, PermissionEntity>();
            CreateMap<PermissionUpdateApiInput, PermissionEntity>();
            CreateMap<PermissionUpdateDotInput, PermissionEntity>();
        }
    }
}
