using ZhonTai.Plate.Admin.Domain.Permission;

namespace ZhonTai.Plate.Admin.Repository
{
    public class PermissionRepository : RepositoryBase<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}