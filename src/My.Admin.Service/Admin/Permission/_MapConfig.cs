using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Permission.Input;
using My.Admin.Service.Admin.Permission.Output;
using AutoMapper;
using System.Linq;

namespace My.Admin.Service.Admin.Permission
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