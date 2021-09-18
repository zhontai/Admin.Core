using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        public RoleRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}