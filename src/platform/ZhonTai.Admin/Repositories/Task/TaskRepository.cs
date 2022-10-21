using FreeScheduler;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Repositories;

public class TaskRepository : AppRepositoryBase<TaskInfo>, ITaskRepository
{
    public TaskRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}