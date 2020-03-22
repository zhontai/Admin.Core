using FreeSql;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin
{
    public class ViewRepository : RepositoryBase<ViewEntity>, IViewRepository
    {
        public ViewRepository(IFreeSql orm, IUnitOfWork uow, IUser user) : base(orm, uow, user)
        {

        }
    }
}
