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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
//using FluentValidation;
//using FluentValidation.AspNetCore;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Auth;
using Admin.Core.Auth;
using Admin.Core.Enums;
using Admin.Core.Filters;
using Admin.Core.Db;
using Admin.Core.Common.Cache;
using PermissionHandler = Admin.Core.Auth.PermissionHandler;
using Admin.Core.Aop;

namespace Admin.Core
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        private readonly AppConfig _appConfig;
        private static string basePath => PlatformServices.Default.Application.ApplicationBasePath;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            _appConfig = new ConfigHelper().Get<AppConfig>("appconfig",env.EnvironmentName) ?? new AppConfig();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region AutoMapper 自动映射
            var ServiceDll = Path.Combine(basePath, "Admin.Core.Service.dll");
            var serviceAssembly = Assembly.LoadFrom(ServiceDll);
            services.AddAutoMapper(serviceAssembly);
            #endregion

            #region Cors 跨域
            services.AddCors(c =>
            {
                c.AddPolicy("Limit", policy =>
                {
                    policy
                    .WithOrigins(_appConfig.Urls)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                services.AddSwaggerGen(c =>
                {
                    typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                    {
                        c.SwaggerDoc(version, new OpenApiInfo
                        {
                            Version = version,
                            Title = "Admin.Core"
                        });
                        //c.OrderActionsBy(o => o.RelativePath);
                    });

                    var xmlModelPath = Path.Combine(basePath, "Admin.Core.Model.xml");
                    c.IncludeXmlComments(xmlModelPath);

                    var xmlPath = Path.Combine(basePath, "Admin.Core.xml");
                    c.IncludeXmlComments(xmlPath, true);

                    var xmlServicesPath = Path.Combine(basePath, "Admin.Core.Service.xml");
                    c.IncludeXmlComments(xmlServicesPath);

                    //添加设置Token的按钮
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Value: Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    //添加Jwt验证设置
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                });
            }
            #endregion

            #region Jwt身份认证
            var jwtConfig = new ConfigHelper().Get<JwtConfig>("jwtconfig", _env.EnvironmentName);
            services.TryAddSingleton(jwtConfig);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();
            services.TryAddSingleton<IUserToken, UserToken>();
            services.AddScoped<IPermissionHandler, PermissionHandler>();

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
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { }); ;
            #endregion

            #region 控制器
            services.AddControllers(options => 
            { 
                options.Filters.Add(typeof(GlobalExceptionFilter));
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
            
            //数据库
            services.AddDb(_env);

            #region 缓存
            var cacheConfig = new ConfigHelper().Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
            if(cacheConfig.Type == CacheType.Redis)
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

            //阻止NLog接收状态消息
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC容器
            
            try
            {
                #region Aop
                var interceptorServiceTypes = new List<Type>();
                if (_appConfig.Aop.Transaction)
                {
                    builder.RegisterType<TransactionInterceptor>();
                    interceptorServiceTypes.Add(typeof(TransactionInterceptor));
                } 
                #endregion

                #region Service
                var servicesDllFile = Path.Combine(basePath, "Admin.Core.Service.dll");
                var assemblysServices = Assembly.LoadFrom(servicesDllFile);

                builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(interceptorServiceTypes.ToArray());
                #endregion

                #region Repository
                var repositoryDllFile = Path.Combine(basePath, "Admin.Core.Repository.dll");
                var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
                builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces();
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            #region app配置
            //异常
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //静态文件
            app.UseStaticFiles();

            //路由
            app.UseRouting();

            //跨域
            app.UseCors("Limit");

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
                    c.RoutePrefix = "";//直接根目录访问
                });
            }
            #endregion
        }
    }
}
