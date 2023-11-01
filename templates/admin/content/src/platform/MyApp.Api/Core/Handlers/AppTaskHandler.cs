using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FreeScheduler;
using FreeSql;
using ZhonTai.Admin.Tools.TaskScheduler;
using ZhonTai.Common.Extensions;
using MyApp.Api.Services.Module;
using MyApp.Api.Core.Consts;
using TaskStatus = FreeScheduler.TaskStatus;

namespace MyApp.Api.Core.Handlers;

/// <summary>
/// 模块任务处理器
/// </summary>
public class AppTaskHandler : CloudTaskHandler
{
    public AppTaskHandler(FreeSqlCloud fsqlc, string dbKey) : base(fsqlc, dbKey)
    {

    }

    /// <summary>
    /// 模块任务
    /// </summary>
    /// <param name="task"></param>
    private static async Task ModuleTask(TaskInfo task)
    {
        using var scope = TaskSchedulerServiceExtensions.ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
        var dics = JsonSerializer.Deserialize<Dictionary<string, string>>(task.Body);
        var moduleId = dics["moduleId"];
        await moduleService.GetAsync(moduleId.ToLong());
        //完成并结束任务
        task.Status = TaskStatus.Completed;
    }

    public override void OnExecuting(Scheduler scheduler, TaskInfo task)
    {
        switch (task.Topic)
        {
            //模块任务
            case TaskNames.ModuleTaskName:
                Task.Run(async () => {
                    await ModuleTask(task);
                }).Wait();
                break;
        }
    }
}