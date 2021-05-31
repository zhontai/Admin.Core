using Admin.Core.Common.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Admin.User.Input
{
    /// <summary>
    /// 更新基本信息
    /// </summary>
    public class UserUpdateBasicInput : Entity
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "请输入昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
