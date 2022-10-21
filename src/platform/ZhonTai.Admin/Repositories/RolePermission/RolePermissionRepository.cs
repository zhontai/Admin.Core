using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.RolePermission;

namespace ZhonTai.Admin.Repositories;

public class RolePermissionRepository : AppRepositoryBase<RolePermissionEntity>, IRolePermissionRepository
{
    public RolePermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}