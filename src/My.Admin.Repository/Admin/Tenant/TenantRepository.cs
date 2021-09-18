using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class TenantRepository : RepositoryBase<TenantEntity>, ITenantRepository
    {
        public TenantRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}