

using FreeSql;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Configs;
using System;
using Admin.Core.Repository.Admin;

namespace Admin.Core.Repository
{
    public class MyUnitOfWorkManager : UnitOfWorkManager
    {
        public MyUnitOfWorkManager(IdleBus<IFreeSql> ib, IServiceProvider serviceProvider) : base(ib.GetFreeSql(serviceProvider))
        {
        }
    }
}
