using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}