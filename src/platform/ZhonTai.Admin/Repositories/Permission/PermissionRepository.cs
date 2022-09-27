using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Repositories;

public class PermissionRepository : RepositoryCloud<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}