using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{	
	public  class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        public RoleRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
