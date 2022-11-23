using FreeScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZhonTai;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Tools.TaskScheduler;
using ZhonTai.ApiUI;

new HostApp(new HostAppOptions
{
	//配置后置服务
	ConfigurePostServices = context =>
	{
        //context.Services.AddTiDb(context);

        //添加任务调度
        context.Services.AddTaskScheduler(DbKeys.AppDb, options =>
        {
            options.ConfigureFreeSql = freeSql =>
            {
                freeSql.CodeFirst
                //配置任务表
                .ConfigEntity<TaskInfo>(a =>
                {
                    a.Name("app_task");
                })
                //配置任务日志表
                .ConfigEntity<TaskLog>(a =>
                {
                    a.Name("app_task_log");
                });
            };

            //模块任务处理器
            options.TaskHandler = new TaskHandler(options.FreeSql);
        });
    },
	//配置后置中间件
	ConfigurePostMiddleware = context =>
    {
		var app = context.App;
		var env = app.Environment;
		var appConfig = app.Services.GetService<AppConfig>();

		#region 新版Api文档
		if (env.IsDevelopment() || appConfig.ApiUI.Enable)
		{
            app.UseApiUI(options =>
            {
                options.RoutePrefix = appConfig.ApiUI.RoutePrefix;
                var routePath = options.RoutePrefix.NotNull() ? $"{options.RoutePrefix}/" : "";
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });
            });
		}
		#endregion
	}
}).Run(args);

#if DEBUG
public partial class Program { }
#endif