using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

#if NETSTANDARD2_0
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#endif

namespace ZhonTai.ApiUI
{
    public class ApiUIMiddleware
    {
        private const string EmbeddedFileNamespace = "ZhonTai.ApiUI.src.dist";

        private readonly ApiUIOptions _options;
        private readonly StaticFileMiddleware _staticFileMiddleware;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiUIMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            ApiUIOptions options)
        {
            _options = options ?? new ApiUIOptions();

            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory, options);

            if (options.JsonSerializerOptions != null)
            {
                _jsonSerializerOptions = options.JsonSerializerOptions;
            }
            else
            {
                _jsonSerializerOptions = new JsonSerializerOptions()
                {
#if NET5_0_OR_GREATER
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
#else
                    IgnoreNullValues = true,
#endif
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false) }
                };
            }
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            var isGet = HttpMethods.IsGet(httpMethod);

            // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
            if (isGet && Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase))
            {
                // Use relative redirect to support proxy environments
                var relativeIndexUrl = string.IsNullOrEmpty(path) || path.EndsWith("/")
                    ? "index.html"
                    : $"{path.Split('/').Last()}/index.html";

                RespondWithRedirect(httpContext.Response, relativeIndexUrl);
                return;
            }

            if (isGet && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            if (isGet && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?swagger-resources$", RegexOptions.IgnoreCase))
            {
                await RespondWithConfig(httpContext.Response);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
        }

        private async Task RespondWithConfig(HttpResponse response)
        {
            await response.WriteAsync(JsonSerializer.Serialize(_options.ConfigObject.Urls, _jsonSerializerOptions));
        }

        private static StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            ApiUIOptions options)
        {
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = string.IsNullOrEmpty(options.RoutePrefix) ? string.Empty : $"/{options.RoutePrefix}",
                FileProvider = new EmbeddedFileProvider(typeof(ApiUIMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace),
            };

            return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
        }

        private static void RespondWithRedirect(HttpResponse response, string location)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = location;
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";

            using (var stream = _options.IndexStream())
            {
                using var reader = new StreamReader(stream);

                // Inject arguments before writing to response
                var htmlBuilder = new StringBuilder(await reader.ReadToEndAsync());
                foreach (var entry in GetIndexArguments())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }

                await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            }
        }

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage(
            "AOT",
            "IL2026:RequiresUnreferencedCode",
            Justification = "Method is only called if the user provides their own custom JsonSerializerOptions.")]
        [UnconditionalSuppressMessage(
            "AOT",
            "IL3050:RequiresDynamicCode",
            Justification = "Method is only called if the user provides their own custom JsonSerializerOptions.")]
#endif
        private Dictionary<string, string> GetIndexArguments()
        {
            string configObject = null;
            string oauthConfigObject = null;
            string interceptors = null;

#if NET6_0_OR_GREATER
            if (_jsonSerializerOptions is null)
            {
                configObject = JsonSerializer.Serialize(_options.ConfigObject, ApiUIOptionsJsonContext.Default.ConfigObject);
                oauthConfigObject = JsonSerializer.Serialize(_options.OAuthConfigObject, ApiUIOptionsJsonContext.Default.OAuthConfigObject);
                interceptors = JsonSerializer.Serialize(_options.Interceptors, ApiUIOptionsJsonContext.Default.InterceptorFunctions);
            }
#endif

            configObject ??= JsonSerializer.Serialize(_options.ConfigObject, _jsonSerializerOptions);
            oauthConfigObject ??= JsonSerializer.Serialize(_options.OAuthConfigObject, _jsonSerializerOptions);
            interceptors ??= JsonSerializer.Serialize(_options.Interceptors, _jsonSerializerOptions);

            return new Dictionary<string, string>()
            {
                { "%(DocumentTitle)", _options.DocumentTitle },
                { "%(HeadContent)", _options.HeadContent },
                { "%(StylesPath)", _options.StylesPath },
                { "%(ScriptBundlePath)", _options.ScriptBundlePath },
                { "%(ScriptPresetsPath)", _options.ScriptPresetsPath },
                { "%(ConfigObject)", configObject },
                { "%(OAuthConfigObject)", oauthConfigObject },
                { "%(Interceptors)", interceptors },
            };
        }
    }
}