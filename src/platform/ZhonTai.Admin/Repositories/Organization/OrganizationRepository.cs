using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Organization;

namespace ZhonTai.Admin.Repositories;

public class OrganizationRepository : RepositoryBase<OrganizationEntity>, IOrganizationRepository
{
    public OrganizationRepository(DbUnitOfWorkManager uowm) : base(uowm)
    {
    }
}