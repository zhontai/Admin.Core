using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.UserStaff;

namespace ZhonTai.Admin.Repositories;

public class StaffRepository : RepositoryBase<UserStaffEntity>, IUserStaffRepository
{
    public StaffRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}