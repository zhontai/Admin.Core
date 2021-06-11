using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class LoginLogRepository : RepositoryBase<LoginLogEntity>, ILoginLogRepository
    {
        public LoginLogRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}