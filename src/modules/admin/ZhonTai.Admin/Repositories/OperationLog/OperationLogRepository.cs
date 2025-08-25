using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.OperationLog;

namespace ZhonTai.Admin.Repositories;

public class OperationLogRepository : LogRepositoryBase<OperationLogEntity>, IOperationLogRepository
{
    public OperationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}