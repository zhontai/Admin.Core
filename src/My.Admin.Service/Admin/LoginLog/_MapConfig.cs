using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.LoginLog.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.LoginLog
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<LoginLogAddInput, LoginLogEntity>();
        }
    }
}