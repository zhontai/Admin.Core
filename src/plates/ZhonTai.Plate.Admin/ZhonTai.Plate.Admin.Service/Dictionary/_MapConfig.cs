using AutoMapper;
using ZhonTai.Plate.Admin.Service.Dictionary.Input;
using ZhonTai.Plate.Admin.Domain.Dictionary;

namespace ZhonTai.Plate.Admin.Service.Dictionary
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