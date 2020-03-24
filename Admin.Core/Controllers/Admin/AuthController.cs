using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Admin.Core.Attributes;
using Admin.Core.Model.Output;
using Admin.Core.Service.Admin.Auth;
using Admin.Core.Service.Admin.Auth.Input;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 授权管理
    /// </summary>
    public class AuthController : AreaController
    {
        
        private readonly IAuthService _authServices;
        
        public AuthController(IAuthService authServices)
        {
            _authServices = authServices;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="lastKey">上次验证码键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetVerifyCode(string lastKey)
        {
            return await _authServices.GetVerifyCodeAsync(lastKey);
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPassWordEncryptKey()
        {
            return await _authServices.GetPassWordEncryptKeyAsync();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Login]
        public async Task<IResponseOutput> GetUserInfo()
        {
            return await _authServices.GetUserInfoAsync();
        }

        /// <summary>
        /// 用户登录
        /// 根据登录信息生成Token
        /// </summary>
        /// <param name="input">登录信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Login(AuthLoginInput input)
        {
            return await _authServices.LoginAsync(input);
        }
    }
}
