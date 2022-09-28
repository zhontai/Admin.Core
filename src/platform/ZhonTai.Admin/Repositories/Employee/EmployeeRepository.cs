using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Employee;

namespace ZhonTai.Admin.Repositories;

public class EmployeeRepository : RepositoryCloud<EmployeeEntity>, IEmployeeRepository
{
    public EmployeeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}