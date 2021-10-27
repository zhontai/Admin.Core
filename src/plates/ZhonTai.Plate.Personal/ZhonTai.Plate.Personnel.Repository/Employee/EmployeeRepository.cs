using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Personnel.Domain.Employee;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}