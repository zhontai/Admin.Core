using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.User;

namespace ZhonTai.Admin.Repositories;

public class UserRepository : AdminRepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {

    }
}