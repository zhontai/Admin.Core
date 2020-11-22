

using FreeSql;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository
{
    public class MyUnitOfWorkManager : UnitOfWorkManager
    {
        public MyUnitOfWorkManager(IdleBus<IFreeSql> ib, IUser user) : base(ib.Get(user.TenantId.Value))
        {
        }
    }
}
