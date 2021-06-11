using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class RolePermissionRepository : RepositoryBase<RolePermissionEntity>, IRolePermissionRepository
    {
        public RolePermissionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}