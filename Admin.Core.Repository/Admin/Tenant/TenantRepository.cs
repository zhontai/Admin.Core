using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class TenantRepository : RepositoryBase<TenantEntity>, ITenantRepository
    {
        public TenantRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}