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
//using FluentValidation;
//using FluentValidation.AspNetCore;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Common.Auth;
using ZhonTai.Tools.Cache;
using ZhonTai.Common.Configs;
using ZhonTai.Common.Consts;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Enums;
using ZhonTai.Admin.Core.Extensions;
using ZhonTai.Admin.Core.Filters;
using ZhonTai.Admin.Core.Logs;
using ZhonTai.Admin.Core.RegisterModules;
using MapsterMapper;
using StackExchange.Profiling;
using System.IO;
using ZhonTai.Tools.DynamicApi;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.Mvc.Controllers;
using ZhonTai.Admin.Core.Attributes;
using Microsoft.AspNetCore.Cors;
using ZhonTai.ApiUI;

namespace ZhonTai.Admin.Core
{
    public abstract class BaseStartup
    {
        protected static string basePath => AppContext.BaseDirectory;
        protected readonly IConfiguration _configuration;
        protected readonly IHostEnvironment _env;
        protected readonly ConfigHelper _configHelper;
        protected readonly AppConfig _appConfig;
        protected const string DefaultCorsPolicyName = "AllowPolicy";

        public BaseStartup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _configHelper = new ConfigHelper();
            _appConfig = _configHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            //雪花漂移算法
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1) { WorkerIdBitLength = 6 });

            //权限处理
            services.AddScoped<IPermissionHandler, PermissionHandler>();

            // ClaimType不被更改
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //用户信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            if (_appConfig.IdentityServer.Enable)
            {
                //is4
                services.TryAddSingleton<IUser, UserIdentiyServer>();
            }
            else
            {
                //jwt
                services.TryAddSingleton<IUser, User>();
            }

            //添加数据库
            services.AddDbAsync(_env).Wait();

            //添加IdleBus单例
            var dbConfig = new ConfigHelper().Get<DbConfig>("dbconfig", _env.EnvironmentName);
            var timeSpan = dbConfig.IdleTime > 0 ? TimeSpan.FromMinutes(dbConfig.IdleTime) : TimeSpan.MaxValue;
            IdleBus<IFreeSql> ib = new IdleBus<IFreeSql>(timeSpan);
            services.AddSingleton(ib);
            //数据库配置
            services.AddSingleton(dbConfig);

            //应用配置
            services.AddSingleton(_appConfig);

            //上传配置
            var uploadConfig = _configHelper.Load("uploadconfig", _env.EnvironmentName, true);
            services.Configure<UploadConfig>(uploadConfig);

            #region Mapster 映射配置

            Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => a.Name.StartsWith("ZhonTai"))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();
            services.AddScoped<IMapper>(sp => new Mapper());
            TypeAdapterConfig.GlobalSettings.Scan(assemblies);

            #endregion Mapster 映射配置

            #region Cors 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, policy =>
                {
                    var hasOrigins = _appConfig.CorUrls?.Length > 0;
                    if (hasOrigins)
                    {
                        policy.WithOrigins(_appConfig.CorUrls);
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

                /*
                //浏览器会发起2次请求,使用OPTIONS发起预检请求，第二次才是api异步请求
                options.AddPolicy("All", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
                */
            });

            #endregion Cors 跨域

            #region 身份认证授权

            var jwtConfig = _configHelper.Get<JwtConfig>("jwtconfig", _env.EnvironmentName);
            services.TryAddSingleton(jwtConfig);

            if (_appConfig.IdentityServer.Enable)
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
                    options.Authority = _appConfig.IdentityServer.Url;
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

            if (_env.IsDevelopment() || _appConfig.Swagger.Enable)
            {
                services.AddSwaggerGen(options =>
                {
                    typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                    {
                        options.SwaggerDoc(version, new OpenApiInfo
                        {
                            Version = version,
                            Title = "ZhonTai.Admin.Host"
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
                    options.DocInclusionPredicate((docName, description) => true);

                    string[] xmlFiles = Directory.GetFiles(basePath, "*.xml");
                    if (xmlFiles.Length > 0)
                    {
                        foreach (var xmlFile in xmlFiles)
                        {
                            options.IncludeXmlComments(xmlFile, true);
                        }
                    }

                    var server = new OpenApiServer()
                    {
                        Url = _appConfig.Swagger.Url,
                        Description = ""
                    };
                    server.Extensions.Add("extensions", new OpenApiObject
                    {
                        ["copyright"] = new OpenApiString(_appConfig.ApiUI.Footer.Content)
                    });
                    options.AddServer(server);

                    #region 添加设置Token的按钮

                    if (_appConfig.IdentityServer.Enable)
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
                                    AuthorizationUrl = new Uri($"{_appConfig.IdentityServer.Url}/connect/authorize"),
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

            if (_appConfig.Log.Operation)
            {
                services.AddScoped<ILogHandler, LogHandler>();
            }

            #endregion 操作日志

            #region 控制器

            services.AddControllers(options =>
            {
                options.Filters.Add<ControllerExceptionFilter>();
                options.Filters.Add<ValidateInputFilter>();
                options.Filters.Add<ValidatePermissionAttribute>();
                if (_appConfig.Log.Operation)
                {
                    options.Filters.Add<ControllerLogFilter>();
                }
                //禁止去除ActionAsync后缀
                //options.SuppressAsyncSuffixInActionNames = false;
            })
            //.AddFluentValidation(config =>
            //{
            //    var assembly = Assembly.LoadFrom(Path.Combine(basePath, "ZhonTai.Admin.Host.dll"));
            //    config.RegisterValidatorsFromAssembly(assembly);
            //})
            .AddNewtonsoftJson(options =>
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

            #region 缓存

            var cacheConfig = _configHelper.Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
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

            if (_appConfig.RateLimit)
            {
                services.AddIpRateLimit(_configuration, cacheConfig);
            }

            #endregion IP限流

            //阻止NLog接收状态消息
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

            //性能分析
            if (_appConfig.MiniProfiler)
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
            });
        }

        /// <summary>
        /// 配置容器
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="Exception"></exception>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC容器
            try
            {
                // 控制器注入
                builder.RegisterModule(new ControllerModule());

                // 单例注入
                builder.RegisterModule(new SingleInstanceModule());

                // 仓储注入
                builder.RegisterModule(new RepositoryModule());

                // 服务注入
                builder.RegisterModule(new ServiceModule(_appConfig));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }

            #endregion AutoFac IOC容器
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        public virtual void Configure(IApplicationBuilder app)
        {
            #region app配置

            //IP限流
            if (_appConfig.RateLimit)
            {
                app.UseIpRateLimiting();
            }

            //性能分析
            if (_appConfig.MiniProfiler)
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
            app.UseCors(DefaultCorsPolicyName);

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            //配置端点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #endregion app配置

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger.Enable)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"ZhonTai.Admin.Host {version}");
                    });
                    c.RoutePrefix = "";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                    //c.DefaultModelsExpandDepth(-1);//不显示Models
                    if (_appConfig.MiniProfiler)
                    {
                        c.InjectJavascript("/swagger/mini-profiler.js?v=4.2.22");
                    }
                });
            }
            #endregion Swagger Api文档

            if (_env.IsDevelopment() || _appConfig.ApiUI.Enable)
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

            //数据库日志
            //var log = LogManager.GetLogger("db");
            //var ei = new LogEventInfo(LogLevel.Error, "", "错误信息");
            //ei.Properties["id"] = YitIdHelper.NextId();
            //log.Log(ei);


        }
    }
}