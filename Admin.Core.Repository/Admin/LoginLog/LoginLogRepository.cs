using FreeSql;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;
using System.Threading.Tasks;

namespace Admin.Core.Repository.Admin
{
    public class LoginLogRepository : RepositoryBase<LoginLogEntity>, ILoginLogRepository
    {
        public LoginLogRepository(IFreeSql orm, IUnitOfWork uow, IUser user) : base(orm, uow, user)
        {
        }
    }
}
