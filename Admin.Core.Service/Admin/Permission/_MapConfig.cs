using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Permission.Input;
using Admin.Core.Service.Admin.Permission.Output;
using AutoMapper;
using System.Linq;

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

            CreateMap<PermissionEntity, PermissionGetDotOutput>().ForMember(
                d => d.ApiIds,
                m => m.MapFrom(s => s.Apis.Select(a => a.Id))
            );
        }
    }
}