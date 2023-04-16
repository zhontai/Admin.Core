using FreeScheduler;
using FreeSql;
using System.Collections.Generic;

namespace ZhonTai.Admin.Tools.TaskScheduler;

public class CloudTaskHandler : ITaskHandler
{
    readonly FreeSqlCloud _fsqlc;
    readonly string _dbKey;
    private IFreeSql _fsql => _fsqlc.Use(_dbKey);
    public CloudTaskHandler(FreeSqlCloud fsqlc, string dbKey)
    {
        _fsqlc = fsqlc;
        _dbKey = dbKey;
    }

    public IEnumerable<TaskInfo> LoadAll() => _fsql.Select<TaskInfo>().Where(a => a.Status == TaskStatus.Running && (a.Round < 0 || a.CurrentRound < a.Round)).ToList();
    public TaskInfo Load(string id) => _fsql.Select<TaskInfo>().Where(a => a.Id == id).First();
    public void OnAdd(TaskInfo task) => _fsql.Insert<TaskInfo>().NoneParameter().AppendData(task).ExecuteAffrows();
    public void OnRemove(TaskInfo task) => _fsql.Delete<TaskInfo>().Where(a => a.Id == task.Id).ExecuteAffrows();
    public void OnExecuted(Scheduler scheduler, TaskInfo task, TaskLog result)
    {
        _fsql.Transaction(() =>
        {
            _fsql.Update<TaskInfo>().NoneParameter().SetSource(task)
                .UpdateColumns(a => new { a.CurrentRound, a.ErrorTimes, a.LastRunTime, a.Status })
                .ExecuteAffrows();
            _fsql.Insert<TaskLog>().NoneParameter().AppendData(result).ExecuteAffrows();
        });
    }

    public virtual void OnExecuting(Scheduler scheduler, TaskInfo task)
    {

    }
}
