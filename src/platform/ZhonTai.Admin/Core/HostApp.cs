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
using ZhonTai.Admin.Core.Db.Transaction;
using Autofac.Core;

namespace ZhonTai.Admin.Core;

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

    public void Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //使用NLog日志
        builder.Host.UseNLog();

        //添加配置
        builder.Host.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("./Configs/ratelimitconfig.json", optional: true, reloadOnChange: true);
            if (context.HostingEnvironment.EnvironmentName.NotNull())
            {
                builder.AddJsonFile($"./Configs/ratelimitconfig.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (context.HostingEnvironment.EnvironmentName.NotNull())
            {
                builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }
        });

        var services = builder.Services;
        var env = builder.Environment;
        var configuration = builder.Configuration;

        var configHelper = new ConfigHelper();
        var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();

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
        services.TryAddSingleton<IUser, User>();

        //数据库配置
        var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);
        services.AddSingleton(dbConfig);

        //添加admin数据库
        var freeSqlCloud = new FreeSqlCloud(dbConfig.DistributeKey);
        services.AddSingleton<IFreeSql>(freeSqlCloud);
        services.AddSingleton(freeSqlCloud);
        services.AddScoped<UnitOfWorkManagerCloud>();
        services.AddAdminDb(freeSqlCloud, env, _hostAppOptions);
        services.AddSingleton(provider => freeSqlCloud.Use(DbKeys.AdminDbKey));


        //上传配置
        var uploadConfig = ConfigHelper.Load("uploadconfig", env.EnvironmentName, true);
        services.Configure<UploadConfig>(uploadConfig);

        //程序集
        Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
          .Where(a => appConfig.AssemblyNames.Contains(a.Name) || a.Name == "ZhonTai.Admin")
          .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

        #region Mapster 映射配置
        services.AddScoped<IMapper>(sp => new Mapper());
        TypeAdapterConfig.GlobalSettings.Scan(assemblies);

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

                options.SchemaFilter<EnumSchemaFilter>();

                options.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });

                options.ResolveConflictingActions(apiDescription => apiDescription.First());
                options.CustomSchemaIds(x => x.FullName);
                //options.DocInclusionPredicate((docName, description) => true);

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
                server.Extensions.Add("extensions", new OpenApiObject
                {
                    ["copyright"] = new OpenApiString(appConfig.ApiUI.Footer.Content)
                });
                options.AddServer(server);

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
        void controllersAction(MvcOptions options)
        {
            options.Filters.Add<ControllerExceptionFilter>();
            options.Filters.Add<ValidateInputFilter>();
            options.Filters.Add<ValidatePermissionAttribute>();
            if (appConfig.Log.Operation)
            {
                options.Filters.Add<ControllerLogFilter>();
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
            AppType.Controllers => services.AddControllers(controllersAction),
            AppType.ControllersWithViews => services.AddControllersWithViews(controllersAction),
            AppType.MVC => services.AddMvc(controllersAction),
            _ => services.AddControllers(controllersAction)
        };

        foreach(var assembly in assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
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

        //异常
        app.UseExceptionHandler("/Error");

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

        //配置端点
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        _hostAppOptions?.ConfigureMiddleware?.Invoke(hostAppMiddlewareContext);

        #region Swagger Api文档
        if (env.IsDevelopment() || appConfig.Swagger.Enable)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    c.SwaggerEndpoint($"/swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });

                c.RoutePrefix = "swagger";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                //c.DefaultModelsExpandDepth(-1);//不显示Models
                if (appConfig.MiniProfiler)
                {
                    c.InjectJavascript("/swagger/mini-profiler.js?v=4.2.22+2.0");
                    c.InjectStylesheet("/swagger/mini-profiler.css?v=4.2.22+2.0");
                }
            });
        }
        #endregion Swagger Api文档

        //数据库日志
        //var log = LogManager.GetLogger("db");
        //var ei = new LogEventInfo(LogLevel.Error, "", "错误信息");
        //ei.Properties["id"] = YitIdHelper.NextId();
        //log.Log(ei);

        _hostAppOptions?.ConfigurePostMiddleware?.Invoke(hostAppMiddlewareContext);
    }
}