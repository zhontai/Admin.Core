using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class LoginLogRepository : RepositoryBase<LoginLogEntity>, ILoginLogRepository
    {
        public LoginLogRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}