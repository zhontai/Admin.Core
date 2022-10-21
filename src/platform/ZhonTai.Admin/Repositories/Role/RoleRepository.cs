using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Repositories;

public class RoleRepository : AppRepositoryBase<RoleEntity>, IRoleRepository
{
    public RoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}