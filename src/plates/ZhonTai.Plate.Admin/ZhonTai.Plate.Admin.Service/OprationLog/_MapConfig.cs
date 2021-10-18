using AutoMapper;
using ZhonTai.Plate.Admin.Service.OprationLog.Input;
using ZhonTai.Plate.Admin.Domain.OprationLog;

namespace ZhonTai.Plate.Admin.Service.OprationLog
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