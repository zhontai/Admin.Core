namespace ZhonTai.Admin.Core.Configs;

public class EmailConfig
{
    public static class Models
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public class EmailModel
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }
        }
    }

    /// <summary>
    /// 主机
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 启用SSL
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 邮箱密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 发件人
    /// </summary>
    public Models.EmailModel FromEmail { get; set; }

    /// <summary>
    /// 收件人
    /// </summary>
    public Models.EmailModel ToEmail { get; set; }
}
