using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Permission;

namespace ZhonTai.Admin.Repositories
{
    public class PermissionRepository : RepositoryBase<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}