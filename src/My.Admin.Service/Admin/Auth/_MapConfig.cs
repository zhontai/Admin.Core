using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Auth.Output;
using AutoMapper;

namespace My.Admin.Service.Admin.Auth
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