using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Admin.Core.Model.Admin;
using Admin.Core.Model.Output;
using Admin.Core.Repository.Admin;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Cache;
using Admin.Core.Service.Admin.Auth.Input;

namespace Admin.Core.Service.Admin.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUser _user;
        private readonly ICache _cache;
        private readonly IUserToken _userToken;
        private readonly IUserRepository _userRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public AuthService(
            IUser user,
            ICache cache,
            IUserToken userToken,
            IUserRepository userRepository,
            IRolePermissionRepository rolePermissionRepository
        )
        {
            _user = user;
            _cache = cache;
            _userToken = userToken;
            _userRepository = userRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IResponseOutput> LoginAsync(AuthLoginInput input)
        {
            if (input.UserName.IsNull())
            {
                return ResponseOutput.NotOk("用户名不能为空！");
            }
            if (input.Password.IsNull())
            {
                return ResponseOutput.NotOk("密码不能为空！");
            }
            if (input.VerifyCode.IsNull())
            {
                return ResponseOutput.NotOk("验证码不能为空！");
            }

            #region 验证码校验
            var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, input.VerifyCodeKey);
            var exists = await _cache.ExistsAsync(verifyCodeKey);
            if (exists)
            {
                var verifyCode = await _cache.GetAsync(verifyCodeKey);
                if (string.IsNullOrEmpty(verifyCode))
                {
                    return ResponseOutput.NotOk("验证码已过期！",1);
                }
                if (verifyCode.ToLower() != input.VerifyCode.ToLower())
                {
                    return ResponseOutput.NotOk("验证码输入有误！",2);
                }
                await _cache.DelAsync(verifyCodeKey);
            }
            else
            {
                return ResponseOutput.NotOk("验证码已过期！", 1);
            }
            #endregion

            var user = (await _userRepository.Select.Where(a => a.UserName == input.UserName).ToOneAsync());
            if (!(user?.Id > 0))
            {
                return ResponseOutput.NotOk("账号输入有误!", 3);
            }

            #region 解密
            if (input.PasswordKey.NotNull())
            {
                var passwordKey = string.Format(CacheKey.PassWordKey, input.PasswordKey);
                var existsPasswordKey = await _cache.ExistsAsync(passwordKey);
                if (existsPasswordKey)
                {
                    var secretKey = await _cache.GetAsync(passwordKey);
                    if (passwordKey.IsNull())
                    {
                        return ResponseOutput.NotOk("解密失败！",1);
                    }
                    input.Password = DesEncrypt.Decrypt(input.Password, secretKey);
                    await _cache.DelAsync(passwordKey);
                }
                else
                {
                    return ResponseOutput.NotOk("解密失败！",1);
                }
            }
            #endregion

            var password = MD5Encrypt.Encrypt32(input.Password);
            if (user.Password != password)
            {
                return ResponseOutput.NotOk("密码输入有误！",4);
            }

            //生成token信息
            var claims = new[]
            {
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.Name.IsNull()?user.NickName:user.Name)
            };
            var token = _userToken.Build(claims);

            return ResponseOutput.Ok(new 
            { 
                token
            });
        }

        public async Task<IResponseOutput> GetUserInfoAsync()
        {
            if (!(_user?.Id > 0))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var user = await _userRepository.Select.WhereDynamic(_user.Id)
                .ToOneAsync(m=>new { 
                    m.NickName,
                    m.Name,
                    m.Avatar
                });

            //获取菜单
            var menus = await _rolePermissionRepository.Select
                .InnerJoin<UserRoleEntity>((a, b) => a.RoleId == b.RoleId && b.UserId == _user.Id)
                .Include(a => a.Permission.View)
                .Where(a => new[] { PermissionType.Group,PermissionType.Menu }.Contains(a.Permission.Type))
                .OrderBy(a => a.Permission.ParentId)
                .OrderBy(a => a.Permission.Sort)
                .Distinct()
                .ToListAsync(a => new
                {
                    a.Permission.Id,
                    a.Permission.ParentId,
                    a.Permission.Path,
                    ViewPath = a.Permission.View.Path,
                    a.Permission.Label,

                    a.Permission.Icon,
                    a.Permission.Opened,
                    a.Permission.Closable,
                    a.Permission.Hidden,
                    a.Permission.NewWindow,
                    a.Permission.External
                });

            return ResponseOutput.Ok(new { user, menus });
        }

        public async Task<IResponseOutput> GetVerifyCodeAsync(string lastKey)
        {
            var img = VerifyCodeHelper.GetBase64String(out string code);

            //删除上次缓存的验证码
            if (lastKey.NotNull())
            {
                await _cache.DelAsync(lastKey);
            }

            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await _cache.SetAsync(key, code, TimeSpan.FromMinutes(5));

            var data = new { key = guid, img };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetPassWordKeyAsync()
        {
            //写入Redis
            var guid = Guid.NewGuid().ToString("N");
            var key = string.Format(CacheKey.PassWordKey, guid);
            var secretKey = StringHelper.GenerateRandom(8);
            await _cache.SetAsync(key, secretKey, TimeSpan.FromMinutes(5));
            var data = new { key = guid, secretKey };

            return ResponseOutput.Ok(data);
        }
    }
}
