using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class UserRoleRepository : RepositoryBase<UserRoleEntity>, IUserRoleRepository
    {
        public UserRoleRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}