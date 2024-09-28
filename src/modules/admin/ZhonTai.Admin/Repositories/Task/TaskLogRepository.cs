using FreeScheduler;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Repositories;

public class TaskLogRepository : RepositoryBase<TaskLog>, ITaskLogRepository
{
    public TaskLogRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.TaskDb, uowm)
    {
    }
}