using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Tenant;

namespace ZhonTai.Admin.Repositories;

public class TenantRepository : RepositoryBase<TenantEntity>, ITenantRepository
{
    public TenantRepository(DbUnitOfWorkManager muowm) : base(muowm)
    {
    }
}