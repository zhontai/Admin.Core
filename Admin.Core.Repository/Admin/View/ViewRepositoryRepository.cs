using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class ViewRepository : RepositoryBase<ViewEntity>, IViewRepository
    {
        public ViewRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}