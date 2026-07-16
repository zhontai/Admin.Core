using ZhonTai.Admin.Contracts.Domain.Sso;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Repositories.Sso;

/// <summary>
/// 单点登录日志仓储
/// </summary>
public class SsoLogRepository : AdminRepositoryBase<SsoLogEntity>, ISsoLogRepository
{
    public SsoLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
