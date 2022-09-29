using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using StackExchange.Profiling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using ZhonTai.Admin.Core.Auth;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Permission;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Tools.Captcha;
using ZhonTai.Admin.Services.LoginLog.Dto;
using ZhonTai.Admin.Services.LoginLog;
using ZhonTai.Admin.Services.User;
using ZhonTai.Common.Extensions;
using ZhonTai.Common.Helpers;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.Auth;

/// <summary>
/// 认证授权服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class AuthService : BaseService, IAuthService, IDynamicApi
{
    private readonly AppConfig _appConfig;
    private readonly JwtConfig _jwtConfig;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly ICaptchaTool _captchaTool;

    public AuthService(
        AppConfig appConfig,
        JwtConfig jwtConfig,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        ITenantRepository tenantRepository,
        ICaptchaTool captchaTool
    )
    {
        _appConfig = appConfig;
        _jwtConfig = jwtConfig;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _tenantRepository = tenantRepository;
        _captchaTool = captchaTool;
    }

    /// <summary>
    /// 获得token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    private string GetToken(AuthLoginOutput user)
    {
        if (user == null)
        {
            return string.Empty;
        }

        var token = LazyGetRequiredService<IUserToken>().Create(new[]
        {
            new Claim(ClaimAttributes.UserId, user.Id.ToString()),
            new Claim(ClaimAttributes.UserName, user.UserName),
            new Claim(ClaimAttributes.Name, user.Name),
            new Claim(ClaimAttributes.TenantId, user.TenantId.ToString()),
            new Claim(ClaimAttributes.TenantType, user.TenantType.ToString()),
            new Claim(ClaimAttributes.DataIsolationType, user.DataIsolationType.ToString())
        });

        return token;
    }

    /// <summary>
    /// 查询密钥
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<IResultOutput> GetPasswordEncryptKeyAsync()
    {
        //写入Redis
        var guid = Guid.NewGuid().ToString("N");
        var key = string.Format(CacheKeys.PassWordEncryptKey, guid);
        var encyptKey = StringHelper.GenerateRandom(8);
        await Cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
        var data = new { key = guid, encyptKey };

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 查询用户信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<IResultOutput> GetUserInfoAsync()
    {
        if (!(User?.Id > 0))
        {
            return ResultOutput.NotOk("未登录！");
        }

        var authUserInfoOutput = new AuthUserInfoOutput
        {
            //用户信息
            User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id),

            //用户菜单
            Menus = await _permissionRepository.Select
            .Where(a => new[] { PermissionTypeEnum.Group, PermissionTypeEnum.Menu }.Contains(a.Type))
            .WhereIf(false, a =>
                _permissionRepository.Orm.Select<RolePermissionEntity>()
                .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                .Where(b => b.PermissionId == a.Id)
                .Any()
            )
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path }),

            //用户权限点
            Permissions = await _permissionRepository.Select
            .Where(a => a.Type == PermissionTypeEnum.Dot)
            .WhereIf(false, a =>
                _permissionRepository.Orm.Select<RolePermissionEntity>()
                .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                .Where(b => b.PermissionId == a.Id)
                .Any()
            )
            .ToListAsync(a => a.Code)
        };

        return ResultOutput.Ok(authUserInfoOutput);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<IResultOutput> LoginAsync(AuthLoginInput input)
    {
        var sw = new Stopwatch();
        sw.Start();

        #region 验证码校验

        if (_appConfig.VarifyCode.Enable)
        {
            input.Captcha.DeleteCache = true;
            input.Captcha.CaptchaKey = CacheKeys.CaptchaKey;
            var isOk = await _captchaTool.CheckAsync(input.Captcha);
            if (!isOk)
            {
                return ResultOutput.NotOk("安全验证不通过，请重新登录");
            }
        }

        #endregion

        #region 密码解密

        if (input.PasswordKey.NotNull())
        {
            var passwordEncryptKey = string.Format(CacheKeys.PassWordEncryptKey, input.PasswordKey);
            var existsPasswordKey = await Cache.ExistsAsync(passwordEncryptKey);
            if (existsPasswordKey)
            {
                var secretKey = await Cache.GetAsync(passwordEncryptKey);
                if (secretKey.IsNull())
                {
                    return ResultOutput.NotOk("解密失败");
                }
                input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
                await Cache.DelAsync(passwordEncryptKey);
            }
            else
            {
                return ResultOutput.NotOk("解密失败！");
            }
        }

        #endregion

        #region 登录
        var password = MD5Encrypt.Encrypt32(input.Password);
        var user = await _userRepository.Select.DisableGlobalFilter("Tenant")
            .Where(a => a.UserName == input.UserName && a.Password == password).ToOneAsync();

        if (!(user?.Id > 0))
        {
            return ResultOutput.NotOk("用户名或密码错误");
        }

        if(user.Status== UserStatusEnum.Disabled)
        {
            return ResultOutput.NotOk("禁止登录，请联系管理员");
        }
        #endregion

        #region 获得token
        var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
        if (_appConfig.Tenant)
        {
            var tenant = await _tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(user.TenantId).ToOneAsync(a => new
            {
                a.TenantType,
                a.DataIsolationType
            });
            authLoginOutput.TenantType = tenant.TenantType;
            authLoginOutput.DataIsolationType = tenant.DataIsolationType;
        }
        string token = GetToken(authLoginOutput); 
        #endregion

        sw.Stop();

        #region 添加登录日志

        var loginLogAddInput = new LoginLogAddInput
        {
            TenantId = authLoginOutput.TenantId,
            Name = authLoginOutput.Name,
            ElapsedMilliseconds = sw.ElapsedMilliseconds,
            Status = true,
            CreatedUserId = authLoginOutput.Id,
            CreatedUserName = input.UserName,
        };

        await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddInput);

        #endregion 添加登录日志

        return ResultOutput.Ok(new { token });
    }

    /// <summary>
    /// 刷新Token
    /// 以旧换新
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IResultOutput> Refresh([BindRequired] string token)
    {
        var jwtSecurityToken = LazyGetRequiredService<IUserToken>().Decode(token);
        var userClaims = jwtSecurityToken?.Claims?.ToArray();
        if (userClaims == null || userClaims.Length == 0)
        {
            return ResultOutput.NotOk();
        }

        var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
        if (refreshExpires.IsNull())
        {
            return ResultOutput.NotOk();
        }

        if (refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
        {
            return ResultOutput.NotOk("登录信息已过期");
        }

        var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
        if (userId.IsNull())
        {
            return ResultOutput.NotOk("登录信息已失效");
        }

        //验签
        var securityKey = _jwtConfig.SecurityKey;
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
        var input = jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload;
        if (jwtSecurityToken.RawSignature != JwtTokenUtilities.CreateEncodedSignature(input, signingCredentials))
        {
            return ResultOutput.NotOk("验签失败");
        }

        var output = await LazyGetRequiredService<IUserService>().GetLoginUserAsync(userId.ToLong());
        string newToken = GetToken(output?.Data);
        return ResultOutput.Ok(new { token = newToken });
    }

    /// <summary>
    /// 获取验证数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    [EnableCors(AdminConsts.AllowAnyPolicyName)]
    public async Task<IResultOutput> GetCaptcha()
    {
        using (MiniProfiler.Current.Step("获取滑块验证"))
        {
            var data = await _captchaTool.GetAsync(CacheKeys.CaptchaKey);
            return ResultOutput.Ok(data);
        }
    }

    /// <summary>
    /// 检查验证数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    [EnableCors(AdminConsts.AllowAnyPolicyName)]
    public async Task<IResultOutput> CheckCaptcha([FromQuery] CaptchaInput input)
    {
        input.CaptchaKey = CacheKeys.CaptchaKey;
        var result = await _captchaTool.CheckAsync(input);
        return ResultOutput.Result(result);
    }
}