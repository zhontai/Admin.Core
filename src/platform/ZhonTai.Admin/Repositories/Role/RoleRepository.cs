using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Repositories;

public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
{
    public RoleRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AdminDb, uowm)
    {
    }
}