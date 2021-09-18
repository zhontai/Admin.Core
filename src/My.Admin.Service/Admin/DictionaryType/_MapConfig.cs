using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.DictionaryType.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.DictionaryType
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DictionaryTypeAddInput, DictionaryTypeEntity>();
            CreateMap<DictionaryTypeUpdateInput, DictionaryTypeEntity>();
        }
    }
}