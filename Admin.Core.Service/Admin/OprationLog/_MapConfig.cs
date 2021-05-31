using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.OprationLog.Input;

namespace Admin.Core.Service.Admin.OprationLog
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
