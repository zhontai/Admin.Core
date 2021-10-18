using AutoMapper;
using ZhonTai.Plate.Admin.Service.LoginLog.Input;
using ZhonTai.Plate.Admin.Domain.LoginLog;

namespace ZhonTai.Plate.Admin.Service.LoginLog
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