using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.PkgPermission;

namespace ZhonTai.Admin.Repositories;

public class PkgPermissionRepository : AdminRepositoryBase<PkgPermissionEntity>, IPkgPermissionRepository
{
    public PkgPermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}