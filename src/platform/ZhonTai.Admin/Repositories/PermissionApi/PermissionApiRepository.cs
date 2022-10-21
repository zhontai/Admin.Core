using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.PermissionApi;

namespace ZhonTai.Admin.Repositories;

public class PermissionApiRepository : AdminRepositoryBase<PermissionApiEntity>, IPermissionApiRepository
{
    public PermissionApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}