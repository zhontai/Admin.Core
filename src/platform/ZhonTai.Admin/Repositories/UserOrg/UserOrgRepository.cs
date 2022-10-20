using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.UserOrg;

namespace ZhonTai.Admin.Repositories;

public class UserOrgRepository : RepositoryBase<UserOrgEntity>, IUserOrgRepository
{
    public UserOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}