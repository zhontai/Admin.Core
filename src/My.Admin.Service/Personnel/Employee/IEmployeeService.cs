using My.Admin.Common.Input;
using My.Admin.Common.Output;
using My.Admin.Domain.Personnel;
using My.Admin.Service.Personnel.Employee.Input;
using My.Admin.Service.Personnel.Employee.Output;
using System.Threading.Tasks;

namespace My.Admin.Service.Personnel.Employee
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