using Admin.Core.Common.Cache;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.Auth.Input;
using Admin.Core.Service.Admin.Auth.Output;
using Admin.Tools.Captcha;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Core.Service.Admin.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppConfig _appConfig;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly VerifyCodeHelper _verifyCodeHelper;
        private readonly ITenantRepository _tenantRepository;
        private readonly ICaptcha _captcha;

        public AuthService(
            AppConfig appConfig,
            VerifyCodeHelper verifyCodeHelper,
            IUserRepository userRepository,
            IPermissionRepository permissionRepository,
            ITenantRepository tenantRepository,
            ICaptcha captcha
        )
        {
            _appConfig = appConfig;
            _verifyCodeHelper = verifyCodeHelper;
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _tenantRepository = tenantRepository;
            _captcha = captcha;
        }

        public async Task<IResponseOutput> GetPassWordEncryptKeyAsync()
        {
            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.PassWordEncryptKey, guid);
            var encyptKey = StringHelper.GenerateRandom(8);
            await Cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
            var data = new { key = guid, encyptKey };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetUserInfoAsync()
        {
            if (!(User?.Id > 0))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var authUserInfoOutput = new AuthUserInfoOutput { };
            //用户信息
            authUserInfoOutput.User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id);

            //用户菜单
            authUserInfoOutput.Menus = await _permissionRepository.Select
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            //用户权限点
            authUserInfoOutput.Permissions = await _permissionRepository.Select
                .Where(a => a.Type == PermissionType.Dot)
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .ToListAsync(a => a.Code);

            return ResponseOutput.Ok(authUserInfoOutput);
        }

        public async Task<IResponseOutput> GetVerifyCodeAsync(string lastKey)
        {
            var img = _verifyCodeHelper.GetBase64String(out string code);

            //删除上次缓存的验证码
            if (lastKey.NotNull())
            {
                await Cache.DelAsync(lastKey);
            }

            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await Cache.SetAsync(key, code, TimeSpan.FromMinutes(5));

            var data = new AuthGetVerifyCodeOutput { Key = guid, Img = img };
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> LoginAsync(AuthLoginInput input)
        {
            #region 验证码校验

            if (_appConfig.VarifyCode.Enable)
            {
                /*
                var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, input.VerifyCodeKey);
                var exists = await Cache.ExistsAsync(verifyCodeKey);
                if (exists)
                {
                    var verifyCode = await Cache.GetAsync(verifyCodeKey);
                    if (string.IsNullOrEmpty(verifyCode))
                    {
                        return ResponseOutput.NotOk("验证码已过期！", 1);
                    }
                    if (verifyCode.ToLower() != input.VerifyCode.ToLower())
                    {
                        return ResponseOutput.NotOk("验证码输入有误！", 2);
                    }
                    await Cache.DelAsync(verifyCodeKey);
                }
                else
                {
                    return ResponseOutput.NotOk("验证码已过期！", 1);
                }
                */
                input.Captcha.DeleteCache = true;
                var isOk = await _captcha.CheckAsync(input.Captcha);
                if (!isOk)
                {
                    return ResponseOutput.NotOk("安全验证不通过，请重新登录！");
                }
            }

            #endregion 验证码校验

            UserEntity user = null;

            user = await _userRepository.Select.DisableGlobalFilter("Tenant").Where(a => a.UserName == input.UserName).ToOneAsync();
            //user = (await _userRepository.GetAsync(a => a.UserName == input.UserName));

            if (!(user?.Id > 0))
            {
                return ResponseOutput.NotOk("账号输入有误!", 3);
            }

            #region 解密

            if (input.PasswordKey.NotNull())
            {
                var passwordEncryptKey = string.Format(CacheKey.PassWordEncryptKey, input.PasswordKey);
                var existsPasswordKey = await Cache.ExistsAsync(passwordEncryptKey);
                if (existsPasswordKey)
                {
                    var secretKey = await Cache.GetAsync(passwordEncryptKey);
                    if (secretKey.IsNull())
                    {
                        return ResponseOutput.NotOk("解密失败！", 1);
                    }
                    input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
                    await Cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    return ResponseOutput.NotOk("解密失败！", 1);
                }
            }

            #endregion 解密

            var password = MD5Encrypt.Encrypt32(input.Password);
            if (user.Password != password)
            {
                return ResponseOutput.NotOk("密码输入有误！", 4);
            }

            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);

            if (_appConfig.Tenant)
            {
                var tenant = await _tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(user.TenantId).ToOneAsync(a => new { a.TenantType, a.DataIsolationType });
                authLoginOutput.TenantType = tenant.TenantType;
                authLoginOutput.DataIsolationType = tenant.DataIsolationType;
            }

            return ResponseOutput.Ok(authLoginOutput);
        }
    }
}