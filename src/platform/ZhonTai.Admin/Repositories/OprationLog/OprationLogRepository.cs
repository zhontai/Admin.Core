using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.OprationLog;

namespace ZhonTai.Admin.Repositories;

public class OprationLogRepository : AdminRepositoryBase<OprationLogEntity>, IOprationLogRepository
{
    public OprationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}