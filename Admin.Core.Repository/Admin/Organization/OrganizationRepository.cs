using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class OrganizationRepository : RepositoryBase<OrganizationEntity>, IOrganizationRepository
    {
        public OrganizationRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}