using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Dictionary.Input;

namespace Admin.Core.Service.Admin.Dictionary
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DictionaryAddInput, DictionaryEntity>();
            CreateMap<DictionaryUpdateInput, DictionaryEntity>();
        }
    }
}
