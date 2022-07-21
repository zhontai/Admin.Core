using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using ZhonTai.Admin.Core;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Enums;
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
				typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
				{
					options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"ZhonTai.Host {version}");
				});
			});
		}
		#endregion
	}
}).Run(args);

#if DEBUG
public partial class Program { }
#endif