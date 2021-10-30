using System.Threading.Tasks;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Personnel.Service.Employee.Input;
using ZhonTai.Plate.Personnel.Service.Employee.Output;
using ZhonTai.Plate.Personnel.Domain.Employee;

namespace ZhonTai.Plate.Personnel.Service.Employee
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public interface IEmployeeService
    {
        Task<ResultOutput<EmployeeGetOutput>> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput<EmployeeEntity> input);

        Task<IResultOutput> AddAsync(EmployeeAddInput input);

        Task<IResultOutput> UpdateAsync(EmployeeUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
    }
}