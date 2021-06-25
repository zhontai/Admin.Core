using Admin.Core.Model.Personnel;

namespace Admin.Core.Repository.Personnel
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}