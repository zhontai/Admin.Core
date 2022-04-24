using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
#if NETSTANDARD2_0
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#endif

namespace ZhonTai.ApiUI
{
    public static class ApiUIBuilderExtensions
    {
        /// <summary>
        /// Register the SwaggerUI middleware with provided options
        /// </summary>
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app, ApiUIOptions options)
        {
            return app.UseMiddleware<ApiUIMiddleware>(options);
        }

        /// <summary>
        /// Register the SwaggerUI middleware with optional setup action for DI-injected options
        /// </summary>
        public static IApplicationBuilder UseApiUI(
            this IApplicationBuilder app,
            Action<ApiUIOptions> setupAction = null)
        {
            ApiUIOptions options;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ApiUIOptions>>().Value;
                setupAction?.Invoke(options);
            }

            // To simplify the common case, use a default that will work with the SwaggerMiddleware defaults
            if (options.ConfigObject.Urls == null)
            {
                var hostingEnv = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                options.ConfigObject.Urls = new[] { new UrlDescriptor { Name = $"{hostingEnv.ApplicationName} v1", Url = "v1/swagger.json" } };
            }

            return app.UseSwaggerUI(options);
        }
    }
}