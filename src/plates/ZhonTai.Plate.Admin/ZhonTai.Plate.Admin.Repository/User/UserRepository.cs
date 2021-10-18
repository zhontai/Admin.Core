using ZhonTai.Plate.Admin.Domain.User;

namespace ZhonTai.Plate.Admin.Repository
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}