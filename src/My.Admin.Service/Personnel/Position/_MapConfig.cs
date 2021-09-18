using My.Admin.Domain.Personnel;
using My.Admin.Service.Personnel.Position.Input;
using AutoMapper;

namespace My.Admin.Service.Personnel.Position
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<PositionAddInput, PositionEntity>();
            //修改
            CreateMap<PositionUpdateInput, PositionEntity>();
        }
    }
}