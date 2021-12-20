using Admin.Core.Attributes;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Consts;
using Admin.Core.Common.Extensions;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Auth;
using Admin.Core.Service.Admin.Auth.Input;
using Admin.Core.Service.Admin.Auth.Output;
using Admin.Core.Service.Admin.LoginLog;
using Admin.Core.Service.Admin.LoginLog.Input;
using Admin.Core.Service.Admin.User;
using Admin.Tools.Captcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 授权管理
    /// </summary>
    public class AuthController : AreaController
    {
        private readonly IUserToken _userToken;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILoginLogService _loginLogService;
        private readonly ICaptcha _captcha;

        public AuthController(
            IUserToken userToken,
            IAuthService authService,
            IUserService userService,
            ILoginLogService loginLogService,
            ICaptcha captcha
        )
        {
            _userToken = userToken;
            _authService = authService;
            _userService = userService;
            _loginLogService = loginLogService;
            _captcha = captcha;
        }

        /// <summary>
        /// 获得token
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private IResponseOutput GetToken(ResponseOutput<AuthLoginOutput> output)
        {
            if (!output.Success)
            {
                return ResponseOutput.NotOk(output.Msg);
            }

            var user = output.Data;

            if (user == null)
            {
                return ResponseOutput.NotOk();
            }

            var token = _userToken.Create(new[]
            {
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.UserName),
                new Claim(ClaimAttributes.UserNickName, user.NickName),
                new Claim(ClaimAttributes.TenantId, user.TenantId.ToString()),
                new Claim(ClaimAttributes.TenantType, user.TenantType.ToString()),
                new Claim(ClaimAttributes.DataIsolationType, user.DataIsolationType.ToString())
            });

            return ResponseOutput.Ok(new { token });
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
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        [EnableCors(AdminConsts.AllowAnyPolicyName)]
        public async Task<IResponseOutput> GetCaptcha()
        {
            var data = await _captcha.GetAsync();
            return ResponseOutput.Ok(data);
        }

        /// <summary>
        /// 检查验证数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        [EnableCors(AdminConsts.AllowAnyPolicyName)]
        public async Task<IResponseOutput> CheckCaptcha([FromQuery] CaptchaInput input)
        {
            var result = await _captcha.CheckAsync(input);
            return ResponseOutput.Result(result);
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
            var res = await _authService.LoginAsync(input);
            sw.Stop();

            #region 添加登录日志

            var loginLogAddInput = new LoginLogAddInput()
            {
                CreatedUserName = input.UserName,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };

            ResponseOutput<AuthLoginOutput> output = null;
            if (res.Success)
            {
                output = (res as ResponseOutput<AuthLoginOutput>);
                var user = output.Data;
                loginLogAddInput.CreatedUserId = user.Id;
                loginLogAddInput.NickName = user.NickName;
                loginLogAddInput.TenantId = user.TenantId;
            }

            await _loginLogService.AddAsync(loginLogAddInput);

            #endregion 添加登录日志

            if (!res.Success)
            {
                return res;
            }

            return GetToken(output);
        }

        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Refresh([BindRequired] string token)
        {
            var userClaims = _userToken.Decode(token);
            if (userClaims == null || userClaims.Length == 0)
            {
                return ResponseOutput.NotOk();
            }

            var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
            if (refreshExpires.IsNull())
            {
                return ResponseOutput.NotOk();
            }

            if (refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
            {
                return ResponseOutput.NotOk("登录信息已过期");
            }

            var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
            if (userId.IsNull())
            {
                return ResponseOutput.NotOk("登录信息已失效");
            }
            var output = await _userService.GetLoginUserAsync(userId.ToLong());

            return GetToken(output);
        }
    }
}