using AutoMapper;
using Admin.Core.Model.Admin;

namespace Admin.Core.Service.Admin.View.Input
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ViewAddInput, ViewEntity>();
            CreateMap<ViewUpdateInput, ViewEntity>();
        }
    }
}
