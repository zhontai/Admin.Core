using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Tenant;

namespace ZhonTai.Admin.Repositories;

public class TenantRepository : RepositoryCloud<TenantEntity>, ITenantRepository
{
    public TenantRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}