using ZhonTai.Plate.Personnel.Service.Employee.Output;
using System.Linq;
using Mapster;
using ZhonTai.Plate.Personnel.Domain.Employee;

namespace ZhonTai.Plate.Personnel.Service.Employee
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
            .NewConfig<EmployeeEntity, EmployeeGetOutput>()
            .Map(dest => dest.OrganizationIds, src => src.Organizations.Select(a => a.Id));

            config
            .NewConfig<EmployeeEntity, EmployeeListOutput>()
            .Map(dest => dest.OrganizationNames, src => src.Organizations.Select(a => a.Name));
        }
    }
}