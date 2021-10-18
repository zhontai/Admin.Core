using ZhonTai.Plate.Admin.Repository;
using ZhonTai.Plate.Personnel.Domain;

namespace ZhonTai.Plate.Personnel.Repository
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntity>, IOrganizationRepository
    {
        public OrganizationRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}