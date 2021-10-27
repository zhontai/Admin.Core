using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Domain.OprationLog;

namespace ZhonTai.Plate.Admin.Repository
{
    public class OprationLogRepository : RepositoryBase<OprationLogEntity>, IOprationLogRepository
    {
        public OprationLogRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}