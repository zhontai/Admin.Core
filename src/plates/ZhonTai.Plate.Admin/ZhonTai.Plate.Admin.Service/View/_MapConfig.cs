using AutoMapper;
using ZhonTai.Plate.Admin.Domain.View;

namespace ZhonTai.Plate.Admin.Service.View.Input
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
            CreateMap<ViewSyncDto, ViewEntity>();
        }
    }
}