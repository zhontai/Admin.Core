using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.DictionaryType.Input;
using AutoMapper;

namespace Admin.Core.Service.Admin.DictionaryType
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