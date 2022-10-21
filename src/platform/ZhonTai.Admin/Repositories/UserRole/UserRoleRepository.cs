using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.UserRole;

namespace ZhonTai.Admin.Repositories;

public class UserRoleRepository : AppRepositoryBase<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}