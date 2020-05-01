using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Admin.Core.Attributes;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Auth;
using Admin.Core.Service.Admin.Auth.Input;
using Admin.Core.Service.Admin.Auth.Output;
using Admin.Core.Common.Auth;
using System.Diagnostics;
using Admin.Core.Service.Admin.LoginLog.Input;
using Admin.Core.Service.Admin.LoginLog;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 授权管理
    /// </summary>
    public class AuthController : AreaController
    {
        private readonly IUserToken _userToken;
        private readonly IAuthService _authService;
        private readonly ILoginLogService _loginLogService;

        public AuthController(
            IUserToken userToken,
            IAuthService authServices,
            ILoginLogService loginLogService
        )
        {
            _userToken = userToken;
            _authService = authServices;
            _loginLogService = loginLogService;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="lastKey">上次验证码键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public async Task<IResponseOutput> GetVerifyCode(string lastKey)
        {
            return await _authService.GetVerifyCodeAsync(lastKey);
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public async Task<IResponseOutput> GetPassWordEncryptKey()
        {
            return await _authService.GetPassWordEncryptKeyAsync();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Login]
        public async Task<IResponseOutput> GetUserInfo()
        {
            return await _authService.GetUserInfoAsync();
        }

        /// <summary>
        /// 用户登录
        /// 根据登录信息生成Token
        /// </summary>
        /// <param name="input">登录信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [NoOprationLog]
        public async Task<IResponseOutput> Login(AuthLoginInput input)
        {
            var sw = new Stopwatch();
            sw.Start();
            var res = (await _authService.LoginAsync(input)) as IResponseOutput;
            sw.Stop();

            #region 添加登录日志
            var loginLogAddInput = new LoginLogAddInput()
            {
                CreatedUserName = input.UserName,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };

            AuthLoginOutput user = null;
            if (res.Success)
            {
                user = (res as IResponseOutput<AuthLoginOutput>).Data;
                loginLogAddInput.CreatedUserId = user.Id;
                loginLogAddInput.NickName = user.NickName;
            }

            await _loginLogService.AddAsync(loginLogAddInput);
            #endregion

            if (!res.Success)
            {
                return res;
            }

            #region 生成token信息
            var token = _userToken.Build(new[]
            {
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.UserName),
                new Claim(ClaimAttributes.UserNickName, user.NickName)
            }); 
            #endregion

            return ResponseOutput.Ok(new { token });
        }
    }
}
