using System;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Common.Configs;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Tools.Captcha;
using ZhonTai.Plate.Admin.Domain.Permission;
using ZhonTai.Plate.Admin.Domain.User;
using ZhonTai.Plate.Admin.Domain.Tenant;
using ZhonTai.Plate.Admin.Service.Auth.Output;
using ZhonTai.Plate.Admin.Domain.RolePermission;
using ZhonTai.Plate.Admin.Domain.UserRole;
using ZhonTai.Plate.Admin.Service.Contracts;
using ZhonTai.Plate.Admin.Service.Auth.Input;

namespace ZhonTai.Plate.Admin.Service.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppConfig _appConfig;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ICaptchaTool _captchaTool;

        public AuthService(
            AppConfig appConfig,
            IUserRepository userRepository,
            IPermissionRepository permissionRepository,
            ITenantRepository tenantRepository,
            ICaptchaTool captchaTool
        )
        {
            _appConfig = appConfig;
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _tenantRepository = tenantRepository;
            _captchaTool = captchaTool;
        }

        public async Task<IResultOutput> GetPassWordEncryptKeyAsync()
        {
            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.PassWordEncryptKey, guid);
            var encyptKey = StringHelper.GenerateRandom(8);
            await Cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
            var data = new { key = guid, encyptKey };

            return ResultOutput.Ok(data);
        }

        public async Task<IResultOutput> GetUserInfoAsync()
        {
            if (!(User?.Id > 0))
            {
                return ResultOutput.NotOk("未登录！");
            }

            var authUserInfoOutput = new AuthUserInfoOutput { };
            //用户信息
            authUserInfoOutput.User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id);

            //用户菜单
            authUserInfoOutput.Menus = await _permissionRepository.Select
                .Where(a => new[] { PermissionTypeEnum.Group, PermissionTypeEnum.Menu }.Contains(a.Type))
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
                .Where(a => a.Type == PermissionTypeEnum.Dot)
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .ToListAsync(a => a.Code);

            return ResultOutput.Ok(authUserInfoOutput);
        }

        public async Task<IResultOutput> LoginAsync(AuthLoginInput input)
        {
            #region 验证码校验

            if (_appConfig.VarifyCode.Enable)
            {
                input.Captcha.DeleteCache = true;
                input.Captcha.CaptchaKey = CacheKey.CaptchaKey;
                var isOk = await _captchaTool.CheckAsync(input.Captcha);
                if (!isOk)
                {
                    return ResultOutput.NotOk("安全验证不通过，请重新登录！");
                }
            }

            #endregion 验证码校验

            UserEntity user = null;

            user = await _userRepository.Select.DisableGlobalFilter("Tenant").Where(a => a.UserName == input.UserName).ToOneAsync();

            if (!(user?.Id > 0))
            {
                return ResultOutput.NotOk("账号输入有误!", 3);
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
                        return ResultOutput.NotOk("解密失败！", 1);
                    }
                    input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
                    await Cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    return ResultOutput.NotOk("解密失败！", 1);
                }
            }

            #endregion 解密

            var password = MD5Encrypt.Encrypt32(input.Password);
            if (user.Password != password)
            {
                return ResultOutput.NotOk("密码输入有误！", 4);
            }

            var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);

            if (_appConfig.Tenant)
            {
                var tenant = await _tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(user.TenantId).ToOneAsync(a => new { a.TenantType, a.DataIsolationType });
                authLoginOutput.TenantType = tenant.TenantType;
                authLoginOutput.DataIsolationType = tenant.DataIsolationType;
            }

            return ResultOutput.Ok(authLoginOutput);
        }
    }
}