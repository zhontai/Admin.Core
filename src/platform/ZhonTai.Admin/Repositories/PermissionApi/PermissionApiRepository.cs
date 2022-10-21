using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.PermissionApi;

namespace ZhonTai.Admin.Repositories;

public class PermissionApiRepository : AppRepositoryBase<PermissionApiEntity>, IPermissionApiRepository
{
    public PermissionApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}