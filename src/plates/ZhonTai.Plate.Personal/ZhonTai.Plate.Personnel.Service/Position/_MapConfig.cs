using ZhonTai.Plate.Personnel.Domain;
using ZhonTai.Plate.Personnel.Service.Position.Input;
using AutoMapper;

namespace ZhonTai.Plate.Personnel.Service.Position
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