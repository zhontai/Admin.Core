using AspNetCoreRateLimit;
using Autofac;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using Mapster;
using Yitter.IdGenerator;
using FluentValidation;
using FluentValidation.AspNetCore;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Tools.Cache;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Extensions;
using ZhonTai.Admin.Core.Filters;
using ZhonTai.Admin.Core.Logs;
using ZhonTai.Admin.Core.RegisterModules;
using System.IO;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.Mvc.Controllers;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using MapsterMapper;
using ZhonTai.DynamicApi;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Admin.Core.Startup;
using ZhonTai.Admin.Core.Conventions;
using FreeSql;
using ZhonTai.Admin.Services.User;
using ZhonTai.Admin.Core.Middlewares;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace ZhonTai.Admin.Core;

/// <summary>
/// 宿主应用
/// </summary>
public class HostApp
{
    readonly HostAppOptions _hostAppOptions;

    public HostApp()
    {
    }

    public HostApp(HostAppOptions hostAppOptions)
    {
        _hostAppOptions = hostAppOptions;
    }

    /// <summary>
    /// 运行应用
    /// </summary>
    /// <param name="args"></param>
    public void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //使用NLog日志
        builder.Host.UseNLog();

        var services = builder.Services;
        var env = builder.Environment;
        var configuration = builder.Configuration;

        var configHelper = new ConfigHelper();
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();

        //添加配置
        builder.Configuration.AddJsonFile("./Configs/ratelimitconfig.json", optional: true, reloadOnChange: true);
        if (env.EnvironmentName.NotNull())
        {
            builder.Configuration.AddJsonFile($"./Configs/ratelimitconfig.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        }
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        if (env.EnvironmentName.NotNull())
        {
            builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        }

        //应用配置
        services.AddSingleton(appConfig);

        //使用Autofac容器
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        //配置Autofac容器
        builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        {
            // 控制器注入
            builder.RegisterModule(new ControllerModule());

            // 单例注入
            builder.RegisterModule(new SingleInstanceModule(appConfig));

            // 仓储注入
            builder.RegisterModule(new RepositoryModule(appConfig));

            // 服务注入
            builder.RegisterModule(new ServiceModule(appConfig));
        });

        //配置Kestrel服务器
        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            //设置应用服务器Kestrel请求体最大为100MB
            options.Limits.MaxRequestBodySize = 1024 * 1024 * 100;
        });

        //访问地址
        builder.WebHost.UseUrls(appConfig.Urls);

        //配置服务
        ConfigureServices(services, env, configuration, configHelper, appConfig);

        var app = builder.Build();

        //配置中间件
        ConfigureMiddleware(app, env, configuration, appConfig);

        app.Run();
    }

    /// <summary>
    /// 实体类型重命名
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    private string DefaultSchemaIdSelector(Type modelType)
    {
        if (!modelType.IsConstructedGenericType) return modelType.Name.Replace("[]", "Array");

        var prefix = modelType.GetGenericArguments()
            .Select(DefaultSchemaIdSelector)
            .Aggregate((previous, current) => previous + current);

        return modelType.Name.Split('`').First() + prefix;
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    /// <param name="configHelper"></param>
    /// <param name="appConfig"></param>
    private void ConfigureServices(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration, ConfigHelper configHelper, AppConfig appConfig)
    {
        var hostAppContext = new HostAppContext()
        {
            Services = services,
            Environment = env,
            Configuration = configuration
        };

        _hostAppOptions?.ConfigurePreServices?.Invoke(hostAppContext);

        //雪花漂移算法
        var idGeneratorOptions = new IdGeneratorOptions(1) { WorkerIdBitLength = 6 };
        _hostAppOptions?.ConfigureIdGenerator?.Invoke(idGeneratorOptions);
        YitIdHelper.SetIdGenerator(idGeneratorOptions);
        
        //权限处理
        services.AddScoped<IPermissionHandler, PermissionHandler>();

        // ClaimType不被更改
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        //用户信息
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddScoped<IUser, User>();

        //数据库配置
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);
        services.AddSingleton(dbConfig);

        //添加数据库
        if (!_hostAppOptions.CustomInitDb)
        {
            services.AddDb(env, _hostAppOptions);
        }

        //上传配置
        var uploadConfig = ConfigHelper.Load("uploadconfig", env.EnvironmentName, true);
        services.Configure<UploadConfig>(uploadConfig);

        //程序集
        Assembly[] assemblies = null;
        if(appConfig.AssemblyNames?.Length > 0)
        {
            assemblies = DependencyContext.Default.RuntimeLibraries
            .Where(a => appConfig.AssemblyNames.Contains(a.Name))
            .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();
        }

        #region Mapster 映射配置
        services.AddScoped<IMapper>(sp => new Mapper());
        if(assemblies?.Length > 0)
        {
            TypeAdapterConfig.GlobalSettings.Scan(assemblies);
        }
        #endregion Mapster 映射配置

        #region Cors 跨域
        services.AddCors(options =>
        {
            options.AddPolicy(AdminConsts.RequestPolicyName, policy =>
            {
                var hasOrigins = appConfig.CorUrls?.Length > 0;
                if (hasOrigins)
                {
                    policy.WithOrigins(appConfig.CorUrls);
                }
                else
                {
                    policy.AllowAnyOrigin();
                }
                policy
                .AllowAnyHeader()
                .AllowAnyMethod();

                if (hasOrigins)
                {
                    policy.AllowCredentials();
                }
            });

            //允许任何源访问Api策略，使用时在控制器或者接口上增加特性[EnableCors(AdminConsts.AllowAnyPolicyName)]
            options.AddPolicy(AdminConsts.AllowAnyPolicyName, policy =>
            {
                policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        #endregion Cors 跨域

        #region 身份认证授权

        var jwtConfig = ConfigHelper.Get<JwtConfig>("jwtconfig", env.EnvironmentName);
        services.TryAddSingleton(jwtConfig);

        if (appConfig.IdentityServer.Enable)
        {
            //is4
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
            })
            .AddJwtBearer(options =>
            {
                options.Authority = appConfig.IdentityServer.Url;
                options.RequireHttpsMetadata = false;
                options.Audience = "admin.server.api";
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
        }
        else
        {
            //jwt
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
        }

        #endregion 身份认证授权

        #region Swagger Api文档

        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            services.AddSwaggerGen(options =>
            {
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerDoc(project.Code.ToLower(), new OpenApiInfo
                    {
                        Title = project.Name,
                        Version = project.Version,
                        Description = project.Description
                    });
                    //c.OrderActionsBy(o => o.RelativePath);
                });

                options.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    var api = controllerAction.AttributeRouteInfo.Template;
                    api = Regex.Replace(api, @"[\{\\\/\}]", "-") + "-" + apiDesc.HttpMethod.ToLower();
                    return api.Replace("--", "-");
                });

                options.ResolveConflictingActions(apiDescription => apiDescription.First());
                options.CustomSchemaIds(modelType => DefaultSchemaIdSelector(modelType));

                //支持多分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    var nonGroup = false;
                    var groupNames = new List<string>();
                    var dynamicApiAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is DynamicApiAttribute);
                    if (dynamicApiAttribute != null)
                    {
                        var dynamicApi = dynamicApiAttribute as DynamicApiAttribute;
                        if(dynamicApi.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(dynamicApi.GroupNames);
                        }
                    }

                    var apiGroupAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is ApiGroupAttribute);
                    if (apiGroupAttribute != null)
                    {
                        var apiGroup = apiGroupAttribute as ApiGroupAttribute;
                        if (apiGroup.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(apiGroup.GroupNames);
                        }
                        nonGroup = apiGroup.NonGroup;
                    }

                    return docName == apiDescription.GroupName || groupNames.Any(a => a == docName) || nonGroup;
                });

                string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                if (xmlFiles.Length > 0)
                {
                    foreach (var xmlFile in xmlFiles)
                    {
                        options.IncludeXmlComments(xmlFile, true);
                    }
                }

                var server = new OpenApiServer()
                {
                    Url = appConfig.Swagger.Url,
                    Description = ""
                };
                if (appConfig.ApiUI.Footer.Enable)
                {
                    server.Extensions.Add("extensions", new OpenApiObject
                    {
                        ["copyright"] = new OpenApiString(appConfig.ApiUI.Footer.Content)
                    });
                }
                options.AddServer(server);

                options.SchemaFilter<EnumSchemaFilter>();

                #region 添加设置Token的按钮

                if (appConfig.IdentityServer.Enable)
                {
                    //添加Jwt验证设置
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    //统一认证
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Description = "oauth2登录授权",
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{appConfig.IdentityServer.Url}/connect/authorize"),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "admin.server.api", "admin后端api" }
                                }
                            }
                        }
                    });
                }
                else
                {
                    //添加Jwt验证设置
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }

                #endregion 添加设置Token的按钮
            });
        }

        #endregion Swagger Api文档

        #region 操作日志

        if (appConfig.Log.Operation)
        {
            services.AddScoped<ILogHandler, LogHandler>();
        }

        #endregion 操作日志

        #region 控制器
        void mvcConfigure(MvcOptions options)
        {
            //options.Filters.Add<ControllerExceptionFilter>();
            options.Filters.Add<ValidateInputFilter>();
            if (appConfig.Validate.Login || appConfig.Validate.Permission)
            {
                options.Filters.Add<ValidatePermissionAttribute>();
            }
            //在具有较高的 Order 值的筛选器之前运行 before 代码
            //在具有较高的 Order 值的筛选器之后运行 after 代码
            if (appConfig.DynamicApi.FormatResult)
            {
                options.Filters.Add<FormatResultFilter>(20);
            }
            if (appConfig.Log.Operation)
            {
                options.Filters.Add<ControllerLogFilter>(10);
            }

            //禁止去除ActionAsync后缀
            //options.SuppressAsyncSuffixInActionNames = false;

            if (env.IsDevelopment() || appConfig.Swagger.Enable)
            {
                //API分组约定
                options.Conventions.Add(new ApiGroupConvention());
            }
        }

        var mvcBuilder = appConfig.AppType switch
        {
            AppType.Controllers => services.AddControllers(mvcConfigure),
            AppType.ControllersWithViews => services.AddControllersWithViews(mvcConfigure),
            AppType.MVC => services.AddMvc(mvcConfigure),
            _ => services.AddControllers(mvcConfigure)
        };

        if (assemblies?.Length > 0)
        {
            foreach (var assembly in assemblies)
            {
                services.AddValidatorsFromAssembly(assembly);
            }
        }
        services.AddFluentValidationAutoValidation();

        mvcBuilder.AddNewtonsoftJson(options =>
        {
            //忽略循环引用
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //使用驼峰 首字母小写
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //设置时间格式
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        })
        .AddControllersAsServices();

        #endregion 控制器

        services.AddHttpClient();

        _hostAppOptions?.ConfigureServices?.Invoke(hostAppContext);

        #region 缓存

        var cacheConfig = ConfigHelper.Get<CacheConfig>("cacheconfig", env.EnvironmentName);
        if (cacheConfig.Type == CacheType.Redis)
        {
            var csredis = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionString);
            RedisHelper.Initialization(csredis);
            services.AddSingleton<ICacheTool, RedisCacheTool>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheTool, MemoryCacheTool>();
        }

        #endregion 缓存

        #region IP限流

        if (appConfig.RateLimit)
        {
            services.AddIpRateLimit(configuration, cacheConfig);
        }

        #endregion IP限流

        //阻止NLog接收状态消息
        services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

        //性能分析
        if (appConfig.MiniProfiler)
        {
            services.AddMiniProfiler();
        }

        //动态api
        services.AddDynamicApi(options =>
        {
            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
            .Where(a => a.Name.EndsWith("Service"))
            .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();
            options.AddAssemblyOptions(assemblies);

            options.FormatResult = appConfig.DynamicApi.FormatResult;
            options.FormatResultType = typeof(ResultOutput<>);

            _hostAppOptions?.ConfigureDynamicApi?.Invoke(options);
        });

        _hostAppOptions?.ConfigurePostServices?.Invoke(hostAppContext);
    }

    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="configuration"></param>
    /// <param name="appConfig"></param>
    private void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env, IConfiguration configuration, AppConfig appConfig)
    {
        var hostAppMiddlewareContext = new HostAppMiddlewareContext()
        {
            App = app,
            Environment = env,
            Configuration = configuration
        };

        _hostAppOptions?.ConfigurePreMiddleware?.Invoke(hostAppMiddlewareContext);

        //异常处理
        app.UseMiddleware<ExceptionMiddleware>();

        //IP限流
        if (appConfig.RateLimit)
        {
            app.UseIpRateLimiting();
        }

        //性能分析
        if (appConfig.MiniProfiler)
        {
            app.UseMiniProfiler();
        }

        //静态文件
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseUploadConfig();

        //路由
        app.UseRouting();

        //跨域
        app.UseCors(AdminConsts.RequestPolicyName);

        //认证
        app.UseAuthentication();

        //授权
        app.UseAuthorization();

        //登录用户初始化数据权限
        if (appConfig.Validate.Permission)
        {
            app.Use(async (ctx, next) =>
            {
                var user = ctx.RequestServices.GetRequiredService<IUser>();
                if (user?.Id > 0)
                {
                    var userService = ctx.RequestServices.GetRequiredService<IUserService>();
                    await userService.GetDataPermissionAsync();
                }

                await next();
            });
        }

        //配置端点
        app.MapControllers();

        _hostAppOptions?.ConfigureMiddleware?.Invoke(hostAppMiddlewareContext);

        #region Swagger Api文档
        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            var routePrefix = appConfig.ApiUI.RoutePrefix;
            if (!appConfig.ApiUI.Enable && routePrefix.IsNull())
            {
                routePrefix = appConfig.Swagger.RoutePrefix;
            }
            var routePath = routePrefix.NotNull() ? $"{routePrefix}/" : "";
            app.UseSwagger(optoins =>
            {
                optoins.RouteTemplate = routePath + optoins.RouteTemplate;
            });
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = appConfig.Swagger.RoutePrefix;
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{routePath}swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });
                
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                //options.DefaultModelsExpandDepth(-1);//不显示Models
                if (appConfig.MiniProfiler)
                {
                    options.InjectJavascript("/swagger/mini-profiler.js?v=4.2.22+2.0");
                    options.InjectStylesheet("/swagger/mini-profiler.css?v=4.2.22+2.0");
                }
            });
        }
        #endregion Swagger Api文档

        _hostAppOptions?.ConfigurePostMiddleware?.Invoke(hostAppMiddlewareContext);
    }
}