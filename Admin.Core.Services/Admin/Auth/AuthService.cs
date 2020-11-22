using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Output;
using Admin.Core.Repository.Admin;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Cache;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Service.Admin.Auth.Input;
using Admin.Core.Service.Admin.Auth.Output;

namespace Admin.Core.Service.Admin.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUser _user;
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly AppConfig _appConfig;
        private readonly VerifyCodeHelper _verifyCodeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;

        public AuthService(
            IUser user,
            ICache cache,
            IMapper mapper,
            AppConfig appConfig,
            VerifyCodeHelper verifyCodeHelper,
            IUserRepository userRepository,
            IPermissionRepository permissionRepository
        )
        {
            _user = user;
            _cache = cache;
            _mapper = mapper;
            _appConfig = appConfig;
            _verifyCodeHelper = verifyCodeHelper;
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
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

            var authLoginOutput = _mapper.Map<AuthLoginOutput>(user);

            return ResponseOutput.Ok(authLoginOutput);
        }

        public async Task<IResponseOutput> GetUserInfoAsync()
        {
            if (!(_user?.Id > 0))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            //用户信息
            var user = await _userRepository.Select.WhereDynamic(_user.Id)
                .ToOneAsync(m => new {
                    m.NickName,
                    m.UserName,
                    m.Avatar
                });

            //用户菜单
            var menus = await _permissionRepository.Select
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == _user.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Path,
                    ViewPath = a.View.Path,
                    a.Label,

                    a.Icon,
                    a.Opened,
                    a.Closable,
                    a.Hidden,
                    a.NewWindow,
                    a.External
                });

            //用户权限点
            var permissions = await _permissionRepository.Select
                .Where(a => new[] { PermissionType.Api, PermissionType.Dot }.Contains(a.Type))
                .Where(a =>
                    _permissionRepository.Orm.Select<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == _user.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                )
                .ToListAsync(a => a.Code);

            return ResponseOutput.Ok(new { user, menus, permissions });
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
