

using FreeSql;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Configs;

namespace Admin.Core.Repository
{
    public class MyUnitOfWorkManager : UnitOfWorkManager
    {
        public MyUnitOfWorkManager(IdleBus<IFreeSql> ib, IUser user,AppConfig appConfig) : base(ib.Get(user.TenantId.Value, appConfig))
        {
        }
    }
}
