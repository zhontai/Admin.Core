using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.LoginLog;

namespace ZhonTai.Admin.Repositories;

public class LoginLogRepository : AppRepositoryBase<LoginLogEntity>, ILoginLogRepository
{
    public LoginLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}