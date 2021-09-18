using My.Admin.Domain.Admin;
using My.Admin.Service.Admin.Document.Input;
using AutoMapper;

namespace My.Admin.Service.Admin.Document
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