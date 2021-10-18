using AutoMapper;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Service.Auth.Output;

namespace ZhonTai.Plate.Admin.Service.Auth
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserEntity, AuthLoginOutput>();
        }
    }
}