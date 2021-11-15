using Mapster;
using System.Linq;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Service.User.Dto;

namespace ZhonTai.Plate.Admin.Service.User
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
            .NewConfig<UserEntity, UserGetOutput>()
            .Map(dest => dest.RoleIds, src => src.Roles.Select(a => a.Id));

            config
            .NewConfig<UserEntity, UserListOutput>()
            .Map(dest => dest.RoleNames, src => src.Roles.Select(a => a.Name));
        }
    }
}