
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Admin.User.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserUpdateInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        public string UserName { get; set; }

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

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
