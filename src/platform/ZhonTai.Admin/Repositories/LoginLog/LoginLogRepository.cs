using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.LoginLog;

namespace ZhonTai.Admin.Repositories;

public class LoginLogRepository : RepositoryBase<LoginLogEntity>, ILoginLogRepository
{
    public LoginLogRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AdminDb, uowm)
    {
    }
}