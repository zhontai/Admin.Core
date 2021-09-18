using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class PermissionRepository : RepositoryBase<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}