using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class ViewRepository : RepositoryBase<ViewEntity>, IViewRepository
    {
        public ViewRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}