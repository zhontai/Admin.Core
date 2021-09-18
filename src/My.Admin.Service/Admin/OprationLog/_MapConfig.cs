using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.OprationLog.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.OprationLog
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<OprationLogAddInput, OprationLogEntity>();
        }
    }
}