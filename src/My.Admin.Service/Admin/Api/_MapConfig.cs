using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Api.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.Api
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