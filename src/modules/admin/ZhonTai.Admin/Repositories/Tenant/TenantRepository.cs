using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Tenant;

namespace ZhonTai.Admin.Repositories;

public class TenantRepository : AdminRepositoryBase<TenantEntity>, ITenantRepository
{
    public TenantRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}