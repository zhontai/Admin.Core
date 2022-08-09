using ZhonTai.Admin.Services.Employee.Output;
using System.Linq;
using Mapster;
using ZhonTai.Admin.Domain.Employee;

namespace ZhonTai.Admin.Services.Employee
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
            .Map(dest => dest.OrganizationIds, src => src.Orgs.Select(a => a.Id));

            config
            .NewConfig<EmployeeEntity, EmployeeListOutput>()
            .Map(dest => dest.OrganizationNames, src => src.Orgs.Select(a => a.Name));
        }
    }
}