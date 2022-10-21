using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Domain.RoleOrg;

namespace ZhonTai.Admin.Repositories;

public class RoleOrgRepository : AppRepositoryBase<RoleOrgEntity>, IRoleOrgRepository
{
    public RoleOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}