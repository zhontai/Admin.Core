using System;

namespace Admin.Core.Service.Admin.Auth.Output
{
    public class AuthLoginOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get; set; }
    }
}
