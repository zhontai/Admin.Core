using FreeScheduler;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Repositories;

public class TaskLogRepository : AppRepositoryBase<TaskLog>, ITaskLogRepository
{
    public TaskLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}