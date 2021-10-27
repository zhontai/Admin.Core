using ZhonTai.Common.Input;
using ZhonTai.Common.Output;
using ZhonTai.Plate.Personnel.Service.Employee.Input;
using ZhonTai.Plate.Personnel.Service.Employee.Output;
using System.Threading.Tasks;
using ZhonTai.Plate.Personnel.Domain.Employee;

namespace ZhonTai.Plate.Personnel.Service.Employee
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