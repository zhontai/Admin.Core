using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Repositories;

public class UserRepository : AppRepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {

    }
}