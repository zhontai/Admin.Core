using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.TenantPermission;

namespace ZhonTai.Admin.Repositories;

public class TenantPermissionRepository : RepositoryBase<TenantPermissionEntity>, ITenantPermissionRepository
{
    public TenantPermissionRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
    {

    }
}