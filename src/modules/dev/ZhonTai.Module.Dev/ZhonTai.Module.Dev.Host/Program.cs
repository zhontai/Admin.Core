using Autofac;
using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Savorboard.CAP.InMemoryMessageQueue;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using ZhonTai;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.ApiUI;
using ZhonTai.Common.Helpers;
using ZhonTai.Module.Dev.Api.Core.Repositories;
using ZhonTai.Module.Dev.Configs;

try
{
    new HostApp(new HostAppOptions()
    {
        //前置配置FreeSql
        ConfigurePreFreeSql = (freeSql, dbConfig) =>
        {
            freeSql.UseJsonMap(); //启用JsonMap功能
        },
        //配置FreeSql构建器
        ConfigureFreeSqlBuilder = (freeSqlBuilder, dbConfig) =>
        {
            //if (dbConfig.Type == FreeSql.DataType.QuestDb)
            //{
            //    freeSqlBuilder.UseQuestDbRestAPI("http://localhost:9000", "admin", "quest");
            //}
        },
        //配置FreeSql
        ConfigureFreeSql = (freeSql, dbConfig) =>
        {
        },
        //配置前置服务
        ConfigurePreServices = context =>
        {
            ZhonTai.Admin.Core.Consts.DbKeys.AdminDb = "admindb";
            context.Services.Configure<CodeGenConfig>(context.Configuration.GetSection("CodeGenConfig"));
        },
        //配置后置服务
        ConfigurePostServices = context =>
        {
            //添加cap事件总线
            var appConfig = AppInfo.GetOptions<AppConfig>();
            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => appConfig.AssemblyNames.Contains(a.Name))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

            context.Services.AddCap(config =>
            {
                config.DefaultGroupName = "MyApp";
                //开发阶段不同开发人员的消息区分，可以通过配置版本号实现
                config.Version = "v1";
                config.FailedRetryCount = 5;
                config.FailedRetryInterval = 15;
                config.EnablePublishParallelSend = true;
                config.UseStorageLock = true;
                config.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                config.UseInMemoryStorage();
                config.UseInMemoryMessageQueue();

                config.FailedThresholdCallback = failed =>
                {
                    AppInfo.Log.Error($@"消息处理失败！类型: {failed.MessageType}, 
已重试 {config.FailedRetryCount} 次仍失败，需人工处理。消息名称: {failed.Message.GetName()}");
                };

                config.UseDashboard();
            }).AddSubscriberAssembly(assemblies);
        },
        //配置Autofac容器
        ConfigureAutofacContainer = (builder, context) =>
        {
            builder.RegisterGeneric(typeof(AppRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
        },
        //配置Mvc
        ConfigureMvcBuilder = (builder, context) =>
        {
        },
        //配置后置中间件
        ConfigurePostMiddleware = context =>
        {
            var app = context.App;
            var env = app.Environment;
            var appConfig = AppInfo.GetOptions<AppConfig>();

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
}
catch(Exception ex)
{
    if(ex.InnerException.Source == "System.Data.SQLite" && ex.Message == "unable to open database file")
    {
        ConsoleHelper.WriteErrorLine(ex.Message);
        ConsoleHelper.WriteWarningLine("请前往 ZhonTai.Module.Dev.Host\\ConfigCenter\\dbconfig.json 文件中，");
        ConsoleHelper.WriteWarningLine("修改多数据库列表 Dbs 中 admindb 权限库所在的 ConnectionString 数据库连接字符串，");
        ConsoleHelper.WriteWarningLine("配置正确的 admindb 权限库绝对路径，如将 E:\\zhontai 替换为你项目所在的跟目录。");
    }
}

public partial class Program { }