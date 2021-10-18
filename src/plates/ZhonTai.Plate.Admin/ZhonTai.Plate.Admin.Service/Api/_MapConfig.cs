using AutoMapper;
using ZhonTai.Plate.Admin.Domain.Api;
using ZhonTai.Plate.Admin.Service.Api.Input;

namespace ZhonTai.Plate.Admin.Service.Admin.Api
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ApiAddInput, ApiEntity>();
            CreateMap<ApiUpdateInput, ApiEntity>();
            CreateMap<ApiSyncDto, ApiEntity>();
        }
    }
}