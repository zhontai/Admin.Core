using FreeScheduler;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Repositories;

public class TaskLogRepository : AppRepositoryBase<TaskLog>, ITaskLogRepository
{
    public TaskLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}