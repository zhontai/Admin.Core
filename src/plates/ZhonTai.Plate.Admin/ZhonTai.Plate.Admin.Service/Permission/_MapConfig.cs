using AutoMapper;
using System.Linq;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Service.Permission.Input;
using ZhonTai.Plate.Admin.Service.Permission.Output;

namespace ZhonTai.Plate.Admin.Service.Admin.Permission
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