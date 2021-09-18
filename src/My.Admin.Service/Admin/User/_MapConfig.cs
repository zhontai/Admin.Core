using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.User.Input;
using My.Admin.Service.Admin.User.Output;
using AutoMapper;
using System.Linq;

namespace My.Admin.Service.Admin.User
{
    /// <summary>
    /// 映射配置
    /// 双向映射 .ReverseMap()
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<UserAddInput, UserEntity>();
            CreateMap<UserUpdateInput, UserEntity>();

            //修改
            CreateMap<UserChangePasswordInput, UserEntity>();
            CreateMap<UserUpdateBasicInput, UserEntity>();

            //查询
            CreateMap<UserEntity, UserGetOutput>().ForMember(
                d => d.RoleIds,
                m => m.MapFrom(s => s.Roles.Select(a => a.Id))
            );

            CreateMap<UserEntity, UserListOutput>().ForMember(
                d => d.RoleNames,
                m => m.MapFrom(s => s.Roles.Select(a => a.Name))
            );
        }
    }
}