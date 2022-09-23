using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Employee;

namespace ZhonTai.Admin.Repositories;

public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
{
    public EmployeeRepository(DbUnitOfWorkManager uowm) : base(uowm)
    {

    }
}