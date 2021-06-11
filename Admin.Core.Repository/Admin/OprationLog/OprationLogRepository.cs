using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class OprationLogRepository : RepositoryBase<OprationLogEntity>, IOprationLogRepository
    {
        public OprationLogRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}