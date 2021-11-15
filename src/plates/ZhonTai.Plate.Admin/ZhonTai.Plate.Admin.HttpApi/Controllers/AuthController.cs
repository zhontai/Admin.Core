using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ZhonTai.Plate.Admin.HttpApi.Shared.Attributes;
using ZhonTai.Common.Auth;
using ZhonTai.Common.Consts;
using ZhonTai.Common.Extensions;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.Auth;
using ZhonTai.Plate.Admin.Service.Auth.Dto;
using ZhonTai.Plate.Admin.Service.LoginLog;
using ZhonTai.Plate.Admin.Service.LoginLog.Dto;
using ZhonTai.Plate.Admin.Service.User;
using ZhonTai.Tools.Captcha;
using ZhonTai.Plate.Admin.Service.Contracts;
using StackExchange.Profiling;

namespace ZhonTai.Plate.Admin.HttpApi
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
        private readonly ICaptchaTool _captcha;

        public AuthController(
            IUserToken userToken,
            IAuthService authService,
            IUserService userService,
            ILoginLogService loginLogService,
            ICaptchaTool captcha
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
        private IResultOutput GetToken(ResultOutput<AuthLoginOutput> output)
        {
            if (!output.Success)
            {
                return ResultOutput.NotOk(output.Msg);
            }

            var user = output.Data;

            if (user == null)
            {
                return ResultOutput.NotOk();
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

            return ResultOutput.Ok(new { token });
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
                var data = await _captcha.GetAsync(CacheKey.CaptchaKey);
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
            input.CaptchaKey = CacheKey.CaptchaKey;
            var result = await _captcha.CheckAsync(input);
            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public async Task<IResultOutput> GetPassWordEncryptKey()
        {
            return await _authService.GetPassWordEncryptKeyAsync();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Login]
        public async Task<IResultOutput> GetUserInfo()
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
        public async Task<IResultOutput> Login(AuthLoginInput input)
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

            ResultOutput<AuthLoginOutput> output = null;
            if (res.Success)
            {
                output = (res as ResultOutput<AuthLoginOutput>);
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
        public async Task<IResultOutput> Refresh([BindRequired] string token)
        {
            var userClaims = _userToken.Decode(token);
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
            var output = await _userService.GetLoginUserAsync(userId.ToLong());

            return GetToken(output);
        }
    }
}