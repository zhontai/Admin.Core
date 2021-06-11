using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class PermissionRepository : RepositoryBase<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}