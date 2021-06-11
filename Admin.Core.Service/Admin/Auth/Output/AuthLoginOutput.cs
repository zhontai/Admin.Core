using Admin.Core.Common.BaseModel;

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
        public long? TenantId { get; set; }

        /// <summary>
        /// 租户类型
        /// </summary>
        public TenantType? TenantType { get; set; }

        /// <summary>
        /// 数据隔离
        /// </summary>
        public DataIsolationType? DataIsolationType { get; set; }
    }
}