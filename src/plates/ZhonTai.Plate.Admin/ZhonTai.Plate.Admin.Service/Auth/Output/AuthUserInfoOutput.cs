using System.Collections.Generic;

namespace ZhonTai.Plate.Admin.Service.Auth.Output
{
    public class AuthUserInfoOutput
    {
        /// <summary>
        /// 用户个人信息
        /// </summary>
        public AuthUserProfileDto User { get; set; }

        /// <summary>
        /// 用户菜单
        /// </summary>
        public List<AuthUserMenuDto> Menus { get; set; }

        /// <summary>
        /// 用户权限点
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}