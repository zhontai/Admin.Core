using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Repositories;

public class PermissionRepository : RepositoryBase<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AdminDb, uowm)
    {
    }
}