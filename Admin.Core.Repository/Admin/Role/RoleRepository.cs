using FreeSql;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{	
	public  class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        public RoleRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
