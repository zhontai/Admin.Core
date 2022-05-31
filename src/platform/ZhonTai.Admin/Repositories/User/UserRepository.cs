using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Repositories
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(DbUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}