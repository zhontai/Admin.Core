using FreeScheduler;
using ZhonTai.Admin.Core.Db.Transaction;

namespace ZhonTai.Admin.Repositories;

public class TaskRepository : AdminRepositoryBase<TaskInfo>, ITaskRepository
{
    public TaskRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}