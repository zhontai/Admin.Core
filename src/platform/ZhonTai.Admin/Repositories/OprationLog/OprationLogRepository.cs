using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.OprationLog;

namespace ZhonTai.Admin.Repositories;

public class OprationLogRepository : RepositoryCloud<OprationLogEntity>, IOprationLogRepository
{
    public OprationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}