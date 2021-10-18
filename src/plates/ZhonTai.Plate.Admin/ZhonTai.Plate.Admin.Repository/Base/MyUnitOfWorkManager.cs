using FreeSql;
using System;

namespace ZhonTai.Plate.Admin.Repository
{
    public class MyUnitOfWorkManager : UnitOfWorkManager
    {
        public MyUnitOfWorkManager(IdleBus<IFreeSql> ib, IServiceProvider serviceProvider) : base(ib.GetFreeSql(serviceProvider))
        {
        }
    }
}