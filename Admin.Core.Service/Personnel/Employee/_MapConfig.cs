using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Employee.Input;
using Admin.Core.Service.Personnel.Employee.Output;
using AutoMapper;
using System.Linq;

namespace Admin.Core.Service.Personnel.Employee
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