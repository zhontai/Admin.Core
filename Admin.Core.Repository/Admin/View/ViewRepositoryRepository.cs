using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{
    public class ViewRepository : RepositoryBase<ViewEntity>, IViewRepository
    {
        public ViewRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {

        }
    }
}
