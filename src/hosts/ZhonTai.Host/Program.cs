using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.ApiUI;

new HostApp(new HostAppOptions
{
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
				options.RoutePrefix = "swagger";
				appConfig.Swagger.Projects?.ForEach(project =>
				{
					options.SwaggerEndpoint($"/swagger/{project.Code.ToLower()}/swagger.json", project.Name);
				});
			});
		}
		#endregion
	}
}).Run(args);

#if DEBUG
public partial class Program { }
#endif