using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.OperationLog;

namespace ZhonTai.Admin.Repositories;

public class OperationLogRepository : AdminRepositoryBase<OperationLogEntity>, IOperationLogRepository
{
    public OperationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}