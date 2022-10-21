using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Repositories;

public class RoleRepository : AdminRepositoryBase<RoleEntity>, IRoleRepository
{
    public RoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}