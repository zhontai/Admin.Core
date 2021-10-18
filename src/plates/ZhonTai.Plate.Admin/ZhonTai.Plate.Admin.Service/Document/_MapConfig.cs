using AutoMapper;
using ZhonTai.Plate.Admin.Domain.Document;
using ZhonTai.Plate.Admin.Domain.DocumentImage;
using ZhonTai.Plate.Admin.Service.Document.Input;

namespace ZhonTai.Plate.Admin.Service.Admin.Document
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DocumentAddGroupInput, DocumentEntity>();
            CreateMap<DocumentAddMenuInput, DocumentEntity>();
            CreateMap<DocumentAddImageInput, DocumentImageEntity>();

            CreateMap<DocumentUpdateGroupInput, DocumentEntity>();
            CreateMap<DocumentUpdateMenuInput, DocumentEntity>();
            CreateMap<DocumentUpdateContentInput, DocumentEntity>();
        }
    }
}