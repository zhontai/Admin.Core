using ZhonTai.Plate.Personnel.Domain;
using ZhonTai.Plate.Personnel.Service.Employee.Input;
using ZhonTai.Plate.Personnel.Service.Employee.Output;
using AutoMapper;
using System.Linq;

namespace ZhonTai.Plate.Personnel.Service.Employee
{
    /// <summary>
    /// 映射配置
    /// 双向映射 .ReverseMap()
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<EmployeeAddInput, EmployeeEntity>();

            //修改
            CreateMap<EmployeeUpdateInput, EmployeeEntity>();

            //查询
            CreateMap<EmployeeEntity, EmployeeGetOutput>().ForMember(
                d => d.OrganizationIds,
                m => m.MapFrom(s => s.Organizations.Select(a => a.Id))
            );

            //查询
            CreateMap<EmployeeEntity, EmployeeGetOutput>().ForMember(
                d => d.OrganizationIds,
                m => m.MapFrom(s => s.Organizations.Select(a => a.Id))
            );

            CreateMap<EmployeeEntity, EmployeeListOutput>().ForMember(
                d => d.OrganizationNames,
                m => m.MapFrom(s => s.Organizations.Select(a => a.Name))
            );
        }
    }
}