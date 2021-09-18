using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class OprationLogRepository : RepositoryBase<OprationLogEntity>, IOprationLogRepository
    {
        public OprationLogRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}