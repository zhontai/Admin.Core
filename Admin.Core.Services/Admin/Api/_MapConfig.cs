using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Api.Input;

namespace Admin.Core.Service.Admin.Api
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
