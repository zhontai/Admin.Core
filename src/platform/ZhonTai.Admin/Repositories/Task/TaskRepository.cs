using FreeScheduler;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Repositories;

public class TaskRepository : AppRepositoryBase<TaskInfo>, ITaskRepository
{
    public TaskRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}