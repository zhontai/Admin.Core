using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Domain.Tenant;

namespace ZhonTai.Plate.Admin.Repository
{
    public class TenantRepository : RepositoryBase<TenantEntity>, ITenantRepository
    {
        public TenantRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}