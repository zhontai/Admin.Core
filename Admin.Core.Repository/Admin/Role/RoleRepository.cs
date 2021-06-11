using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        public RoleRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}