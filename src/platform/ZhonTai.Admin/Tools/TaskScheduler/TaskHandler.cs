using FreeScheduler;
using System;
using System.Collections.Generic;

namespace ZhonTai.Admin.Tools.TaskScheduler;

public class TaskHandler : ITaskHandler
{
    internal IFreeSql _fsql;
    public TaskHandler(IFreeSql fsql)
    {
        _fsql = fsql;
        // 此处使用 Aop 设置实体，外部可通过 FluentApi 复盖重新设置新值
        _fsql.Aop.ConfigEntity += (_, e) =>
        {
            if (e.EntityType == typeof(TaskInfo)) e.ModifyResult.Name = "app_task";
            if (e.EntityType == typeof(TaskLog)) e.ModifyResult.Name = "app_task_log";
        };
        _fsql.Aop.ConfigEntityProperty += (_, e) =>
        {
            if (e.EntityType == typeof(TaskInfo))
            {
                switch (e.Property.Name)
                {
                    case nameof(TaskInfo.Id): e.ModifyResult.IsPrimary = true; break;
                    case nameof(TaskInfo.Body): e.ModifyResult.StringLength = -1; break;
                    case nameof(TaskInfo.Interval): e.ModifyResult.MapType = typeof(string); break;
                    case nameof(TaskInfo.IntervalArgument): e.ModifyResult.StringLength = 1024; break;
                    case nameof(TaskInfo.Status): e.ModifyResult.MapType = typeof(string); break;
                }
            }
            else if (e.EntityType == typeof(TaskLog))
            {
                switch (e.Property.Name)
                {
                    case nameof(TaskLog.Exception): e.ModifyResult.StringLength = -1; break;
                    case nameof(TaskLog.Remark): e.ModifyResult.StringLength = -1; break;
                }
            }
        };
        _fsql.CodeFirst.SyncStructure<TaskInfo>();
        _fsql.CodeFirst.SyncStructure<TaskLog>();
    }

    public IEnumerable<TaskInfo> LoadAll(int pageNumber, int pageSize) => _fsql.Select<TaskInfo>()
        .Where(a => a.Status == TaskStatus.Running && (a.Round < 0 || a.CurrentRound < a.Round))
        .OrderBy(a => a.Id)
        .Page(pageNumber, pageSize).ToList();
    public TaskInfo Load(string id) => _fsql.Select<TaskInfo>().Where(a => a.Id == id).First();
    public void OnAdd(TaskInfo task) => _fsql.Insert<TaskInfo>().NoneParameter().AppendData(task).ExecuteAffrows();
    public void OnRemove(TaskInfo task)
    {
        _fsql.Transaction(() =>
        {
            _fsql.Delete<TaskLog>().Where(a => a.TaskId == task.Id).ExecuteAffrows();
            _fsql.Delete<TaskInfo>().Where(a => a.Id == task.Id).ExecuteAffrows();
        });
    }
    public void OnExecuted(Scheduler scheduler, TaskInfo task, TaskLog result)
    {
        try
        {
            _fsql.Transaction(() =>
            {
                _fsql.Update<TaskInfo>().NoneParameter().SetSource(task)
                    .UpdateColumns(a => new { a.CurrentRound, a.ErrorTimes, a.LastRunTime, a.Status })
                    .ExecuteAffrows();
                _fsql.Insert<TaskLog>().NoneParameter().AppendData(result).ExecuteAffrows();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {task.Topic} FreeSqlHandler.OnExecuted 错误：{ex.Message}");
        }
    }

    public virtual void OnExecuting(Scheduler scheduler, TaskInfo task)
    {
        Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {task.Topic} 被执行，还剩 {scheduler.QuantityTask} 个循环任务");
    }
}
