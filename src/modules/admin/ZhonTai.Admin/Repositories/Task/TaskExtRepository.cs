using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain;

namespace ZhonTai.Admin.Repositories;

public class TaskExtRepository : RepositoryBase<TaskInfoExt>, ITaskExtRepository
{
    public TaskExtRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.TaskDb, uowm)
    {
    }
}