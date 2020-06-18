using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Admin.Core.Common.Configs;
using Microsoft.Extensions.FileProviders;

namespace Admin.Core.Extensions
{
    public static class UploadConfigApplicationBuilderExtensions
    {
        private static void UseFileUploadConfig(IApplicationBuilder app, FileUploadConfig config)
        {
            if (!Directory.Exists(config.UploadPath))
            {
                Directory.CreateDirectory(config.UploadPath);
            }

            app.UseStaticFiles(new StaticFileOptions() 
            {
                RequestPath = config.RequestPath,
                FileProvider = new PhysicalFileProvider(config.UploadPath)
            });
        }

        public static IApplicationBuilder UseUploadConfig(this IApplicationBuilder app)
        {
            var uploadConfig = app.ApplicationServices.GetRequiredService<IOptions<UploadConfig>>();
            UseFileUploadConfig(app, uploadConfig.Value.Avatar);
            UseFileUploadConfig(app, uploadConfig.Value.Document);

            return app;
        }
    }

}
