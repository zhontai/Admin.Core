using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}