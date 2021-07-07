using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Employee.Input;
using Admin.Core.Service.Personnel.Employee.Output;
using System.Threading.Tasks;

namespace Admin.Core.Service.Personnel.Employee
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public interface IEmployeeService
    {
        Task<ResponseOutput<EmployeeGetOutput>> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<EmployeeEntity> input);

        Task<IResponseOutput> AddAsync(EmployeeAddInput input);

        Task<IResponseOutput> UpdateAsync(EmployeeUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids);
    }
}