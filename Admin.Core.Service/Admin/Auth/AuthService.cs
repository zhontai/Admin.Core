using System;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Output;
using Admin.Core.Repository.Admin;
using Admin.Core.Common.Cache;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Service.Admin.Auth.Input;
using Admin.Core.Service.Admin.Auth.Output;

namespace Admin.Core.Service.Admin.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly ICache _cache;
        private readonly AppConfig _appConfig;
        private readonly VerifyCodeHelper _verifyCodeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ITenantRepository _tenantRepository;

        public AuthService(
            ICache cache,
            AppConfig appConfig,
            VerifyCodeHelper verifyCodeHelper,
            IUserRepository userRepository,
            IPermissionRepository permissionRepository,
            ITenantRepository tenantRepository
        )
        {
            _cache = cache;
            _appConfig = appConfig;
            _verifyCodeHelper = verifyCodeHelper;
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<IResponseOutput> LoginAsync(AuthLoginInput input)
        {
            #region 验证码校验
            if (_appConfig.VarifyCode.Enable)
            {
                var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, input.VerifyCodeKey);
                var exists = await _cache.ExistsAsync(verifyCodeKey);
                if (exists)
                {
                    var verifyCode = await _cache.GetAsync(verifyCodeKey);
                    if (string.IsNullOrEmpty(verifyCode))
                    {
                        return ResponseOutput.NotOk("验证码已过期！", 1);
                    }
                    if (verifyCode.ToLower() != input.VerifyCode.ToLower())
                    {
                        return ResponseOutput.NotOk("验证码输入有误！", 2);
                    }
                    await _cache.DelAsync(verifyCodeKey);
                }
                else
                {
                    return ResponseOutput.NotOk("验证码已过期！", 1);
                }
            }
            #endregion

            UserEntity user = null;

            user = await _userRepository.Select.DisableGlobalFilter("Tenant").Where(a=> a.UserName == input.UserName).ToOneAsync();
            //user = (await _userRepository.GetAsync(a => a.UserName == input.UserName));

            if (!(user?.Id > 0))
            {
                return ResponseOutput.NotOk("账号输入有误!", 3);
            }

            #region 解密
            if (input.PasswordKey.NotNull())
            {
                var passwordEncryptKey = string.Format(CacheKey.PassWordEncryptKey, input.PasswordKey);
                var existsPasswordKey = await _cache.ExistsAsync(passwordEncryptKey);
                if (existsPasswordKey)
                {
                    var secretKey = await _cache.GetAsync(passwordEncryptKey);
                    if (secretKey.IsNull())
                    {
                        return ResponseOutput.NotOk("解密失败！", 1);
                    }
                    input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
                    await _cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    return ResponseOutput.NotOk("解密失败！", 1);
                }
            }
            #endregion

            var password = MD5Encrypt.Encrypt32(input.Password);
            if (user.Password != password)
            {
                return ResponseOutput.NotOk("密码输入有误！", 4);
            }

            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);

            if(_appConfig.TenantDbType == TenantDbType.Share)
            {
                authLoginOutput.TenantType = await _tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(user.TenantId).ToOneAsync(a => a.TenantType);
            }

            return ResponseOutput.Ok(authLoginOutput);
        }

        public async Task<IResponseOutput> GetUserInfoAsync()
        {
            if (!(User?.Id > 0))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var key = string.Format(CacheKey.UserInfo, User.Id);
            var output = await _cache.GetOrSetAsync(key, async () => 
            {
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
                    .Where(a => new[] { PermissionType.Api, PermissionType.Dot }.Contains(a.Type))
                    .Where(a =>
                        _permissionRepository.Orm.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    )
                    .ToListAsync(a => a.Code);

                return authUserInfoOutput;
            });
            

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> GetVerifyCodeAsync(string lastKey)
        {
            var img = _verifyCodeHelper.GetBase64String(out string code);

            //删除上次缓存的验证码
            if (lastKey.NotNull())
            {
                await _cache.DelAsync(lastKey);
            }

            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await _cache.SetAsync(key, code, TimeSpan.FromMinutes(5));

            var data = new AuthGetVerifyCodeOutput { Key = guid, Img = img };
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetPassWordEncryptKeyAsync()
        {
            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.PassWordEncryptKey, guid);
            var encyptKey = StringHelper.GenerateRandom(8);
            await _cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
            var data = new { key = guid, encyptKey };

            return ResponseOutput.Ok(data);
        }
    }
}
