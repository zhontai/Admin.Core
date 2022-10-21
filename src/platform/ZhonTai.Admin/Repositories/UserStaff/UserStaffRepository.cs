using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.UserStaff;

namespace ZhonTai.Admin.Repositories;

public class UserStaffRepository : AdminRepositoryBase<UserStaffEntity>, IUserStaffRepository
{
    public UserStaffRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}