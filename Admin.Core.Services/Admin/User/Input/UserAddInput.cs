using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Admin.User.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class UserAddInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string Avatar { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public long[] RoleIds { get; set; }
    }
}
