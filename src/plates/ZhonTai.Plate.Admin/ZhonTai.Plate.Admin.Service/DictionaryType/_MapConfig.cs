using AutoMapper;
using ZhonTai.Plate.Admin.Service.DictionaryType.Input;
using ZhonTai.Plate.Admin.Domain.DictionaryType;

namespace ZhonTai.Plate.Admin.Service.DictionaryType
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