using System.Threading.Tasks;
using Admin.Core.Service.Admin.User;
using Admin.Core.Common.Output;
using Admin.Core.Common.Input;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.User.Input;
using Microsoft.AspNetCore.Mvc;
using Admin.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Microsoft.Extensions.Options;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : AreaController
    {
        private readonly IUser _user;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;
        private readonly IUserService _userServices;

        public UserController(
            IUser user,
            IOptionsMonitor<UploadConfig> uploadConfig,
            UploadHelper uploadHelper, 
            IUserService userServices
        )
        {
            _user = user;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
            _userServices = userServices;
        }

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetBasic()
        {
            return await _userServices.GetBasicAsync();
        }

        /// <summary>
        /// 查询单条用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _userServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[ResponseCache(Duration = 60)]
        public async Task<IResponseOutput> GetPage(PageInput<UserEntity> input)
        {
            return await _userServices.PageAsync(input);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(UserAddInput input)
        {
            return await _userServices.AddAsync(input);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(UserUpdateInput input)
        {
            return await _userServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _userServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _userServices.BatchSoftDeleteAsync(ids);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ChangePassword(UserChangePasswordInput input)
        {
            return await _userServices.ChangePasswordAsync(input);
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateBasic(UserUpdateBasicInput input)
        {
            return await _userServices.UpdateBasicAsync(input);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResponseOutput> AvatarUpload([FromForm]IFormFile file)
        {
            var config = _uploadConfig.Avatar;
            var res = await _uploadHelper.UploadAsync(file, config, new { _user.Id });
            if (res.Success)
            {
                return ResponseOutput.Ok(res.Data.FileRelativePath);
            }

            return ResponseOutput.NotOk("上传失败！");
        }
    }
}