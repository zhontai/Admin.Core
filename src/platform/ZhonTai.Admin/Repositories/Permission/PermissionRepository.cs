using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Repositories;

public class PermissionRepository : AdminRepositoryBase<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}