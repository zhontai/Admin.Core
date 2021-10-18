using ZhonTai.Plate.Admin.Repository;
using ZhonTai.Plate.Personnel.Domain;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}