using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Organization;

namespace ZhonTai.Admin.Repositories;

public class OrganizationRepository : RepositoryCloud<OrganizationEntity>, IOrganizationRepository
{
    public OrganizationRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}