using Admin.Core.Model.Personnel;

namespace Admin.Core.Repository.Personnel
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntity>, IOrganizationRepository
    {
        public OrganizationRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}