using ZhonTai.Admin.Contracts.Domain.Sso;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Repositories.Sso;

/// <summary>
/// 单点登录应用仓储
/// </summary>
public class SsoAppManageRepository : AdminRepositoryBase<SsoAppManageEntity>, ISsoAppManageRepository
{
    public SsoAppManageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
