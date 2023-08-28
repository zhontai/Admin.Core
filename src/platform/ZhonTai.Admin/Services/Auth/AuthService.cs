using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
using ZhonTai.Admin.Services.LoginLog.Dto;
using ZhonTai.Admin.Services.LoginLog;
using ZhonTai.Admin.Services.User;
using ZhonTai.Common.Extensions;
using ZhonTai.Common.Helpers;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using FreeSql;
using ZhonTai.Admin.Domain.TenantPermission;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ZhonTai.Admin.Core.Captcha;
using Newtonsoft.Json;
using Lazy.SlideCaptcha.Core.Validator;
using static Lazy.SlideCaptcha.Core.ValidateResult;
using ZhonTai.Admin.Domain.PkgPermission;
using ZhonTai.Admin.Domain.TenantPkg;

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
    private IPasswordHasher<UserEntity> _passwordHasher => LazyGetRequiredService<IPasswordHasher<UserEntity>>();
    private ISlideCaptcha _captcha => LazyGetRequiredService<ISlideCaptcha>();

    public AuthService(
        AppConfig appConfig,
        JwtConfig jwtConfig,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        ITenantRepository tenantRepository
    )
    {
        _appConfig = appConfig;
        _jwtConfig = jwtConfig;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _tenantRepository = tenantRepository;
    }

    /// <summary>
    /// 获得token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    [NonAction]
    public string GetToken(AuthLoginOutput user)
    {
        if (user == null)
        {
            return string.Empty;
        }

        var claims = new List<Claim>()
       {
            new Claim(ClaimAttributes.UserId, user.Id.ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimAttributes.UserName, user.UserName),
            new Claim(ClaimAttributes.Name, user.Name),
            new Claim(ClaimAttributes.UserType, user.Type.ToInt().ToString(), ClaimValueTypes.Integer32),
        };

        if (_appConfig.Tenant)
        {
            claims.AddRange(new []
            {
                new Claim(ClaimAttributes.TenantId, user.TenantId.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimAttributes.TenantType, user.Tenant?.TenantType.ToInt().ToString(), ClaimValueTypes.Integer32),
                new Claim(ClaimAttributes.DbKey, user.Tenant?.DbKey ?? "")
            });
        }

        var token = LazyGetRequiredService<IUserToken>().Create(claims.ToArray());

        return token;
    }

    /// <summary>
    /// 查询密钥
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<AuthGetPasswordEncryptKeyOutput> GetPasswordEncryptKeyAsync()
    {
        //写入Redis
        var guid = Guid.NewGuid().ToString("N");
        var key = CacheKeys.PassWordEncrypt + guid;
        var encyptKey = StringHelper.GenerateRandom(8);
        await Cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
        return new AuthGetPasswordEncryptKeyOutput { Key = guid, EncyptKey = encyptKey };
    }

    /// <summary>
    /// 查询用户个人信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthUserProfileDto> GetUserProfileAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var profile = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id);

            return profile;
        }
    }
   
    /// <summary>
    /// 查询用户菜单列表
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<List<AuthUserMenuDto>> GetUserMenusAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var menuSelect = _permissionRepository.Select;

            if (!User.PlatformAdmin)
            {
                var db = _permissionRepository.Orm;
                if (User.TenantAdmin)
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                   );
                }
                else
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<RolePermissionEntity>()
                       .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                       .Where(b => b.PermissionId == a.Id)
                       .Any()
                   );
                }

                menuSelect = menuSelect.AsTreeCte(up: true);
            }

            var menuList = await menuSelect
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            return menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

        }
    }

    /// <summary>
    /// 查询用户权限列表
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserPermissionsOutput> GetUserPermissionsAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var authGetUserPermissionsOutput = new AuthGetUserPermissionsOutput
            {
                //用户信息
                User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id)
            };

            var dotSelect = _permissionRepository.Select.Where(a => a.Type == PermissionType.Dot);

            if (!User.PlatformAdmin)
            {
                var db = _permissionRepository.Orm;
                if (User.TenantAdmin)
                {
                    dotSelect = dotSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                    );
                }
                else
                {
                    dotSelect = dotSelect.Where(a =>
                        db.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    );
                }
            }

            //用户权限点
            authGetUserPermissionsOutput.Permissions = await dotSelect.ToListAsync(a => a.Code);

            return authGetUserPermissionsOutput;
        }
    }

    /// <summary>
    /// 查询用户信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserInfoOutput> GetUserInfoAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var authGetUserInfoOutput = new AuthGetUserInfoOutput
            {
                //用户信息
                User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id)
            };

            var menuSelect = _permissionRepository.Select;
            var dotSelect = _permissionRepository.Select.Where(a => a.Type == PermissionType.Dot);

            if (!User.PlatformAdmin)
            {
                var db = _permissionRepository.Orm;
                if (User.TenantAdmin)
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                   );

                    dotSelect = dotSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                    );
                }
                else
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<RolePermissionEntity>()
                       .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                       .Where(b => b.PermissionId == a.Id)
                       .Any()
                   );

                    dotSelect = dotSelect.Where(a =>
                        db.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    );
                }

                menuSelect = menuSelect.AsTreeCte(up: true);
            }

            var menuList = await menuSelect
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            //用户菜单
            authGetUserInfoOutput.Menus = menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

            //用户权限点
            authGetUserInfoOutput.Permissions = await dotSelect.ToListAsync(a => a.Code);

            return authGetUserInfoOutput;
        }
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<dynamic> LoginAsync(AuthLoginInput input)
    {
        using (_userRepository.DataFilter.DisableAll())
        {
            var stopwatch = Stopwatch.StartNew();

            #region 验证码校验

            if (_appConfig.VarifyCode.Enable)
            {
                if(input.CaptchaId.IsNull() || input.CaptchaData.IsNull())
                {
                    throw ResultOutput.Exception("请完成安全验证");
                }
                var validateResult = _captcha.Validate(input.CaptchaId, JsonConvert.DeserializeObject<SlideTrack>(input.CaptchaData));
                if (validateResult.Result != ValidateResultType.Success)
                {
                    throw ResultOutput.Exception($"安全{validateResult.Message}，请重新登录");
                }
            }

            #endregion

            #region 密码解密

            if (input.PasswordKey.NotNull())
            {
                var passwordEncryptKey = CacheKeys.PassWordEncrypt + input.PasswordKey;
                var existsPasswordKey = await Cache.ExistsAsync(passwordEncryptKey);
                if (existsPasswordKey)
                {
                    var secretKey = await Cache.GetAsync(passwordEncryptKey);
                    if (secretKey.IsNull())
                    {
                        throw ResultOutput.Exception("解密失败");
                    }
                    input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
                    await Cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    throw ResultOutput.Exception("解密失败");
                }
            }

            #endregion

            #region 登录
            var user = await _userRepository.Select.Where(a => a.UserName == input.UserName).ToOneAsync();
            var valid = user?.Id > 0;
            if(valid)
            {
                if (user.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
                {
                    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);
                    valid = passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded;
                }
                else
                {
                    var password = MD5Encrypt.Encrypt32(input.Password);
                    valid = user.Password == password;
                }
            }
            
            if(!valid)
            {
                throw ResultOutput.Exception("用户名或密码错误");
            }

            if (!user.Enabled)
            {
                throw ResultOutput.Exception("账号已停用，禁止登录");
            }
            #endregion

            #region 获得token
            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
            if (_appConfig.Tenant)
            {
                var tenant = await _tenantRepository.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantDto>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw ResultOutput.Exception("企业已停用，禁止登录");
                }
                authLoginOutput.Tenant = tenant;
            }
            string token = GetToken(authLoginOutput);
            #endregion

            stopwatch.Stop();

            #region 添加登录日志

            var loginLogAddInput = new LoginLogAddInput
            {
                TenantId = authLoginOutput.TenantId,
                Name = authLoginOutput.Name,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                Status = true,
                CreatedUserId = authLoginOutput.Id,
                CreatedUserName = user.UserName,
            };

            await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddInput);

            #endregion 添加登录日志

            return new { token };
        }
    }

    /// <summary>
    /// 手机号登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<dynamic> MobileLoginAsync(AuthMobileLoginInput input)
    {
        using (_userRepository.DataFilter.DisableAll())
        {
            var stopwatch = Stopwatch.StartNew();

            #region 短信验证码验证
            if (input.CodeId.IsNull() || input.Code.IsNull())
            {
                throw ResultOutput.Exception("验证码错误");
            }
            var codeKey = CacheKeys.GetSmsCodeKey(input.Mobile, input.CodeId);
            var code = await Cache.GetAsync(codeKey);
            if (code.IsNull())
            {
                throw ResultOutput.Exception("验证码错误");
            }
            await Cache.DelAsync(codeKey);
            if (code != input.Code)
            {
                throw ResultOutput.Exception("验证码错误");
            }

            #endregion

            #region 登录
            var user = await _userRepository.Select.Where(a => a.Mobile == input.Mobile).ToOneAsync();
            if (!(user?.Id > 0))
            {
                throw ResultOutput.Exception("账号不存在");
            }

            if (!user.Enabled)
            {
                throw ResultOutput.Exception("账号已停用，禁止登录");
            }
            #endregion

            #region 获得token
            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
            if (_appConfig.Tenant)
            {
                var tenant = await _tenantRepository.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthLoginTenantDto>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw ResultOutput.Exception("企业已停用，禁止登录");
                }
                authLoginOutput.Tenant = tenant;
            }
            string token = GetToken(authLoginOutput);
            #endregion

            stopwatch.Stop();

            #region 添加登录日志

            var loginLogAddInput = new LoginLogAddInput
            {
                TenantId = authLoginOutput.TenantId,
                Name = authLoginOutput.Name,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                Status = true,
                CreatedUserId = authLoginOutput.Id,
                CreatedUserName = user.UserName,
            };

            await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddInput);

            #endregion 添加登录日志

            return new { token };
        }
    }

    /// <summary>
    /// 刷新Token
    /// 以旧换新
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<dynamic> Refresh([BindRequired] string token)
    {
        var jwtSecurityToken = LazyGetRequiredService<IUserToken>().Decode(token);
        var userClaims = jwtSecurityToken?.Claims?.ToArray();
        if (userClaims == null || userClaims.Length == 0)
        {
            throw ResultOutput.Exception("无法解析token");
        }

        var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
        if (refreshExpires.IsNull() || refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
        {
            throw ResultOutput.Exception("登录信息已过期");
        }

        var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
        if (userId.IsNull())
        {
            throw ResultOutput.Exception("登录信息已失效");
        }

        //验签
        var securityKey = _jwtConfig.SecurityKey;
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
        var input = jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload;
        if (jwtSecurityToken.RawSignature != JwtTokenUtilities.CreateEncodedSignature(input, signingCredentials))
        {
            throw ResultOutput.Exception("验签失败");
        }

        var user = await LazyGetRequiredService<IUserService>().GetLoginUserAsync(userId.ToLong());
        if(!(user?.Id > 0))
        {
            throw ResultOutput.Exception("账号不存在");
        }
        if (!user.Enabled)
        {
            throw ResultOutput.Exception("账号已停用，禁止登录");
        }

        if (_appConfig.Tenant)
        {
            if (!(user.Tenant != null && user.Tenant.Enabled))
            {
                throw ResultOutput.Exception("企业已停用，禁止登录");
            }
        }

        string newToken = GetToken(user);
        return new { token = newToken };
    }

    /// <summary>
    /// 是否开启验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public bool IsCaptcha()
    {
        return _appConfig.VarifyCode.Enable;
    }
}