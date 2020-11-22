using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{
    public class RolePermissionRepository : RepositoryBase<RolePermissionEntity>, IRolePermissionRepository
    {
        public RolePermissionRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }

}

