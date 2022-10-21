using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Tenant;

namespace ZhonTai.Admin.Repositories;

public class TenantRepository : AppRepositoryBase<TenantEntity>, ITenantRepository
{
    public TenantRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}