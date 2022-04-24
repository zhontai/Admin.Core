using System.Threading.Tasks;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Employee.Input;
using ZhonTai.Admin.Services.Employee.Output;
using ZhonTai.Admin.Domain.Employee;

namespace ZhonTai.Admin.Services.Employee
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public interface IEmployeeService
    {
        Task<ResultOutput<EmployeeGetOutput>> GetAsync(long id);

        Task<IResultOutput> GetPageAsync(PageInput input);

        Task<IResultOutput> AddAsync(EmployeeAddInput input);

        Task<IResultOutput> UpdateAsync(EmployeeUpdateInput input);

        Task<IResultOutput> DeleteAsync(long id);

        Task<IResultOutput> SoftDeleteAsync(long id);

        Task<IResultOutput> BatchSoftDeleteAsync(long[] ids);
    }
}