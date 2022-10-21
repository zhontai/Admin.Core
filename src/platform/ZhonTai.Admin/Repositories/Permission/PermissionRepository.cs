using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Repositories;

public class PermissionRepository : AppRepositoryBase<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}