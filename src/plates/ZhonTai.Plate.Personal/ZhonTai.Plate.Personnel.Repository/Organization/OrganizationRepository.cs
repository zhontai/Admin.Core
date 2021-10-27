using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Personnel.Domain.Organization;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntity>, IOrganizationRepository
    {
        public OrganizationRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}