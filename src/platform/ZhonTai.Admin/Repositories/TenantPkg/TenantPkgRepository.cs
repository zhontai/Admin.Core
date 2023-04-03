using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.TenantPkg;

namespace ZhonTai.Admin.Repositories;

public class TenantPkgRepository : AdminRepositoryBase<TenantPkgEntity>, ITenantPkgRepository
{
    public TenantPkgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}