using My.Admin.Domain.Personnel;

namespace My.Admin.Repository.Personnel
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}