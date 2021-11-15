using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ZhonTai.Plate.Admin.HttpApi.Shared.Attributes;
using ZhonTai.Common.Auth;
using ZhonTai.Common.Configs;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Domain.Dto;
using ZhonTai.Plate.Admin.Service.User;
using ZhonTai.Plate.Admin.Service.User.Dto;
using ZhonTai.Plate.Admin.Domain.User;

namespace ZhonTai.Plate.Admin.HttpApi
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : AreaController
    {
        private readonly IUser _user;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;
        private readonly IUserService _userService;

        public UserController(
            IUser user,
            IOptionsMonitor<UploadConfig> uploadConfig,
            UploadHelper uploadHelper,
            IUserService userService
        )
        {
            _user = user;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
            _userService = userService;
        }

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> GetBasic()
        {
            return await _userService.GetBasicAsync();
        }

        /// <summary>
        /// 查询单条用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> Get(long id)
        {
            return await _userService.GetAsync(id);
        }

        /// <summary>
        /// 查询下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> GetSelect()
        {
            return await _userService.GetSelectAsync();
        }

        /// <summary>
        /// 查询分页用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ResponseCache(Duration = 60)]
        public async Task<IResultOutput> GetPage(PageInput input)
        {
            return await _userService.GetPageAsync(input);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> Add(UserAddInput input)
        {
            return await _userService.AddAsync(input);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> Update(UserUpdateInput input)
        {
            return await _userService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultOutput> SoftDelete(long id)
        {
            return await _userService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> BatchSoftDelete(long[] ids)
        {
            return await _userService.BatchSoftDeleteAsync(ids);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> ChangePassword(UserChangePasswordInput input)
        {
            return await _userService.ChangePasswordAsync(input);
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> UpdateBasic(UserUpdateBasicInput input)
        {
            return await _userService.UpdateBasicAsync(input);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResultOutput> AvatarUpload([FromForm] IFormFile file)
        {
            var config = _uploadConfig.Avatar;
            var res = await _uploadHelper.UploadAsync(file, config, new { _user.Id });
            if (res.Success)
            {
                return ResultOutput.Ok(res.Data.FileRelativePath);
            }

            return ResultOutput.NotOk(res.Msg ?? "上传失败！");
        }
    }
}