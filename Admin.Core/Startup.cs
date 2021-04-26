using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
//using FluentValidation;
//using FluentValidation.AspNetCore;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Configs;
using Admin.Core.Auth;
using Admin.Core.Enums;
using Admin.Core.Filters;
using Admin.Core.Db;
using Admin.Core.Common.Cache;
using Admin.Core.Aop;
using Admin.Core.Logs;
using Admin.Core.Extensions;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Auth;
using AspNetCoreRateLimit;
using IdentityServer4.AccessTokenValidation;
using System.IdentityModel.Tokens.Jwt;

namespace Admin.Core
{
    public class Startup
    {
        private static string basePath => AppContext.BaseDirectory;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        private readonly ConfigHelper _configHelper;
        private readonly AppConfig _appConfig;
        private const string DefaultCorsPolicyName = "Allow";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _configHelper = new ConfigHelper();
            _appConfig = _configHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        public void ConfigureServices(IServiceCollection services)
        {
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

            //主数据库
            services.AddDbAsync(_env).Wait();
            //租户数据库
            services.AddTenantDb(_env);

            //应用配置
            services.AddSingleton(_appConfig);

            //上传配置
            var uploadConfig = _configHelper.Load("uploadconfig", _env.EnvironmentName, true);
            services.Configure<UploadConfig>(uploadConfig);

            #region AutoMapper 自动映射
            var serviceAssembly = Assembly.Load("Admin.Core.Service");
            services.AddAutoMapper(serviceAssembly);
            #endregion

            #region Cors 跨域
            if (_appConfig.CorUrls?.Length > 0)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(DefaultCorsPolicyName, policy =>
                    {
                        policy
                        .WithOrigins(_appConfig.CorUrls)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });

                    /*
                    //浏览器会发起2次请求,使用OPTIONS发起预检请求，第二次才是api异步请求
                    options.AddPolicy("All", policy =>
                    {
                        policy
                        .AllowAnyOrigin()
                        .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                    */
                });
            }
            #endregion

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
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                services.AddSwaggerGen(options =>
                {
                    typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                    {
                        options.SwaggerDoc(version, new OpenApiInfo
                        {
                            Version = version,
                            Title = "Admin.Core"
                        });
                        //c.OrderActionsBy(o => o.RelativePath);
                    });

                    var xmlPath = Path.Combine(basePath, "Admin.Core.xml");
                    options.IncludeXmlComments(xmlPath, true);

                    var xmlCommonPath = Path.Combine(basePath, "Admin.Core.Common.xml");
                    options.IncludeXmlComments(xmlCommonPath, true);

                    var xmlModelPath = Path.Combine(basePath, "Admin.Core.Model.xml");
                    options.IncludeXmlComments(xmlModelPath);

                    var xmlServicesPath = Path.Combine(basePath, "Admin.Core.Service.xml");
                    options.IncludeXmlComments(xmlServicesPath);

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
                    #endregion
                });
            }
            #endregion

            #region 操作日志
            if (_appConfig.Log.Operation)
            {
                //services.AddSingleton<ILogHandler, LogHandler>();
                services.AddScoped<ILogHandler, LogHandler>();
            }
            #endregion

            #region 控制器
            services.AddControllers(options =>
            {
                options.Filters.Add<AdminExceptionFilter>();
                if (_appConfig.Log.Operation)
                {
                    options.Filters.Add<LogActionFilter>();
                }
                //禁止去除ActionAsync后缀
                options.SuppressAsyncSuffixInActionNames = false;
            })
            //.AddFluentValidation(config =>
            //{
            //    var assembly = Assembly.LoadFrom(Path.Combine(basePath, "Admin.Core.dll"));
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
            });
            #endregion

            #region 缓存
            var cacheConfig = _configHelper.Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
            if (cacheConfig.Type == CacheType.Redis)
            {
                var csredis = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            #endregion

            #region IP限流
            if (_appConfig.RateLimit)
            {
                services.AddIpRateLimit(_configuration, cacheConfig);
            } 
            #endregion

            //阻止NLog接收状态消息
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC容器
            try
            {
                #region SingleInstance
                //无接口注入单例
                var assemblyCore = Assembly.Load("Admin.Core");
                var assemblyCommon = Assembly.Load("Admin.Core.Common");
                builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon)
                .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
                .SingleInstance();

                //有接口注入单例
                builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon)
                .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
                .AsImplementedInterfaces()
                .SingleInstance();
                #endregion

                #region Aop
                var interceptorServiceTypes = new List<Type>();
                if (_appConfig.Aop.Transaction)
                {
                    builder.RegisterType<TransactionInterceptor>();
                    builder.RegisterType<TransactionAsyncInterceptor>();
                    interceptorServiceTypes.Add(typeof(TransactionInterceptor));
                }
                #endregion

                #region Repository
                var assemblyRepository = Assembly.Load("Admin.Core.Repository");
                builder.RegisterAssemblyTypes(assemblyRepository)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();// 属性注入
                #endregion

                #region Service
                var assemblyServices = Assembly.Load("Admin.Core.Service");
                builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired()// 属性注入
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
            #endregion
        }

        public void Configure(IApplicationBuilder app)
        {
            #region app配置
            //IP限流
            if (_appConfig.RateLimit)
            {
                app.UseIpRateLimiting();
            }

            //跨域
            if (_appConfig.CorUrls?.Length > 0)
            {
                app.UseCors(DefaultCorsPolicyName);
            }

            //异常
            app.UseExceptionHandler("/Error");

            //静态文件
            app.UseUploadConfig();

            //路由
            app.UseRouting();

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            //配置端点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Admin.Core {version}");
                    });
                    c.RoutePrefix = "";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                    //c.DefaultModelsExpandDepth(-1);//不显示Models
                });
            }
            #endregion
        }
    }
}