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
        private static void UseAvatar(IApplicationBuilder app, AvatarConfig avatarConfig)
        {
            if (!Directory.Exists(avatarConfig.Path))
            {
                Directory.CreateDirectory(avatarConfig.Path);
            }

            app.UseStaticFiles(new StaticFileOptions() 
            {
                RequestPath = avatarConfig.RequestPath,
                FileProvider = new PhysicalFileProvider(avatarConfig.Path)
            });
        }

        public static void UseUploadConfig(this IApplicationBuilder app)
        {
            var uploadConfig = app.ApplicationServices.GetRequiredService<IOptions<UploadConfig>>();
            var avatar = uploadConfig.Value.Avatar;
            UseAvatar(app, avatar);
        }
    }

}
