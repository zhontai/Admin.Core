using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.TenantPermission;

namespace ZhonTai.Admin.Repositories;

public class TenantPermissionRepository : AdminRepositoryBase<TenantPermissionEntity>, ITenantPermissionRepository
{
    public TenantPermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}