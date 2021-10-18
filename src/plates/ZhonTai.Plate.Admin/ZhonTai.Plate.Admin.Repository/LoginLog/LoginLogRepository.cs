using ZhonTai.Plate.Admin.Domain.LoginLog;

namespace ZhonTai.Plate.Admin.Repository
{
    public class LoginLogRepository : RepositoryBase<LoginLogEntity>, ILoginLogRepository
    {
        public LoginLogRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}