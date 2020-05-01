using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Document.Input;

namespace Admin.Core.Service.Admin.Document
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
