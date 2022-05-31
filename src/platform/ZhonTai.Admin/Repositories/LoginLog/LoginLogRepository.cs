using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.LoginLog;

namespace ZhonTai.Admin.Repositories
{
    public class LoginLogRepository : RepositoryBase<LoginLogEntity>, ILoginLogRepository
    {
        public LoginLogRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}